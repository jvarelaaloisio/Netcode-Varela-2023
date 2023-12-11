using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Extensions;
using EventChannels.Runtime.Additions.Ids;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Unity.Netcode;

using Events.Runtime.Channels;
using Events.Runtime.Channels.Helpers;
using Scenery;

namespace Management
{
    [Icon("Assets/Gizmos/GameManager Icon.png")]
    public class GameManager : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] private NetworkManager networkManager;
        [SerializeField]private SceneContainer mainMenu;
        [Header("Levels")]
        [SerializeField] private SceneContainer lobby;

        [SerializeField] private LevelWithId[] levels;
        private readonly Dictionary<Id, SceneContainer> _levelsById = new();
        private SceneContainer _currentLevel;

        [Header("Channels Listened")]
        [SerializeField] private VoidChannelSo hostChannel;
        [SerializeField] private VoidChannelSo joinChannel;
        [SerializeField] private VoidChannelSo disconnectChannel;
        [SerializeField] private VoidChannelSo quitChannel;
        [SerializeField] private IdChannelSo goToNextLevelChannel;

        [Header("Events")]
        public UnityEvent onHost;
        public UnityEvent onJoin;
        public UnityEvent onDisconnect;
        public UnityEvent<SceneContainer> onUnloadingLevel;
        public UnityEvent<SceneContainer> onLoadedLevel;
        
        private readonly ISceneManager _unitySceneManager = new UnitySceneManagerFacade();
        private ISceneManager _sceneManager;
        private LevelManager _currentLevelManager;
        private void OnValidate()
        {
            mainMenu.Validate();
            lobby.Validate();
            InitializeLevelsDictionary();
            foreach (var pair in _levelsById)
                pair.Value.Validate();
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            if (networkManager == null)
            {
                Debug.LogError($"{name}: {nameof(networkManager)} is null!" +
                               "\nDisabling");
                enabled = false;
                return;
            }

            mainMenu.Config(_unitySceneManager);
            mainMenu.Load();
            InitializeLevelsDictionary();
            if (levels.Length > _levelsById.Count)
            {
                this.LogWarning($"The {nameof(levels)} provided seems to have duplicated Ids." +
                                $"\nThis is not allowed!");
            }
        }

        private void OnEnable()
        {
            hostChannel.TrySubscribe(HostGame);
            joinChannel.TrySubscribe(JoinGame);
            disconnectChannel.TrySubscribe(Disconnect);
            quitChannel.TrySubscribe(QuitGame);
            goToNextLevelChannel.TrySubscribe(GoToNextLevel);
        }

        private void OnDisable()
        {
            hostChannel.TryUnsubscribe(HostGame);
            joinChannel.TryUnsubscribe(JoinGame);
            disconnectChannel.TryUnsubscribe(Disconnect);
            quitChannel.TryUnsubscribe(QuitGame);
        }

        /// <summary>
        /// Adds all levels from the levels list to the internal dictionary
        /// </summary>
        private void InitializeLevelsDictionary()
        {
            foreach (var current in levels)
            {
                if (!current.ID || current.Container == null)
                    continue;
                if (!_levelsById.ContainsKey(current.ID))
                    _levelsById.Add(current.ID, current.Container);
            }
        }

        private void GoToNextLevel(Id levelId)
        {
            GoToNextLevelServerRPC(levelId.name);
        }

        [ServerRpc]
        private void GoToNextLevelServerRPC(string idName)
        {
            StartCoroutine(DespawnCurrentLevelThenGoToNext(idName));
        }

        /// <summary>
        /// Despawns all objects from current level manager
        /// </summary>
        /// <param name="idName"></param>
        /// <returns></returns>
        private IEnumerator DespawnCurrentLevelThenGoToNext(string idName)
        {
            if (_currentLevelManager)
                yield return _currentLevelManager.DespawnAllAsHost();
            GoToNextLevelClientRPC(idName);
        }
        
        [ClientRpc]
        private void GoToNextLevelClientRPC(string idName)
        {
            var levelPair = _levelsById.FirstOrDefault(pair => pair.Key.name == idName);
            if (levelPair.Value != null)
                StartCoroutine(GoToNextLevelCoroutine(levelPair.Value));
            else
                this.LogError($"No level assigned to Id [{idName}]");
        }

        private IEnumerator GoToNextLevelCoroutine(SceneContainer nextLevel)
        {
            yield return LoadLevel(nextLevel);
            yield return FindLevelManager(nextLevel.Name);
            if (!_currentLevelManager)
            {
                this.LogError($"{_currentLevelManager} was not found in level {nextLevel.Name}. <color=red>Aborting!</color>");
                yield break;
            }
            if (networkManager.IsHost)
                yield return _currentLevelManager.InitAsHost();
            yield return WaitForPlayerToSpawnAndSetItUp();
        }

        /// <summary>
        /// Starts host coroutine
        /// </summary>
        private void HostGame() => StartCoroutine(HostGameCoroutine());

        /// <summary>
        /// Starts Game as Host
        /// </summary>
        private IEnumerator HostGameCoroutine()
        {
            
            networkManager.StartHost();

            yield return StartGame(onHost.Invoke, lobby);

            yield return FindLevelManager(lobby.Name);
            
            yield return _currentLevelManager.InitAsHost();
            
            yield return WaitForPlayerToSpawnAndSetItUp();
            
            networkManager.OnClientDisconnectCallback += HandleClientDisconnected;
        }

        private void HandleClientDisconnected(ulong clientId)
        {
            if (!networkManager.IsHost)
                return;
            Disconnect();
        }

        /// <summary>
        /// Starts Join coroutine
        /// </summary>
        private void JoinGame()
        {
            StartCoroutine(JoinGameCoroutine());
        }

        /// <summary>
        /// Connects to game as client
        /// </summary>
        private IEnumerator JoinGameCoroutine()
        {
            networkManager.StartClient();
            
            yield return StartGame(onJoin.Invoke, lobby);
            
            yield return FindLevelManager(lobby.Name);
            
            yield return new WaitUntil(() => networkManager.LocalClient != null);
            
            yield return new WaitUntil(() => networkManager.LocalClient.PlayerObject != null);
            
            yield return WaitForPlayerToSpawnAndSetItUp();
        }

        /// <summary>
        /// Loads <see cref="lobbyLevel"/> and subscribes to <see cref="NetworkManager.OnClientStopped"/>
        /// </summary>
        /// <param name="onFinish"></param>
        /// <param name="lobbyLevel"></param>
        private IEnumerator StartGame(Action onFinish, SceneContainer lobbyLevel)
        {
            yield return LoadLevel(lobbyLevel);
            networkManager.OnClientStopped += HandleClientStopped;
            onFinish?.Invoke();
        }

        /// <summary>
        /// Unloads current and loads next level
        /// </summary>
        /// <param name="newLevel">level to load</param>
        /// <returns></returns>
        private IEnumerator LoadLevel(SceneContainer newLevel)
        {
            onUnloadingLevel.Invoke(_currentLevel);
            _currentLevel?.Unload();
            newLevel.Config(new UnitySceneManagerFacade());
            yield return newLevel.LoadAsync();
            _currentLevel = newLevel;
            onLoadedLevel.Invoke(newLevel);
        }

        /// <summary>
        /// Called when disconnecting
        /// </summary>
        /// <param name="wasHost"></param>
        private void HandleClientStopped(bool wasHost)
        {
            _currentLevel.Unload();
            _currentLevel = null;
            networkManager.OnClientStopped -= HandleClientStopped;
            networkManager.OnClientDisconnectCallback -= HandleClientDisconnected;
            onDisconnect.Invoke();
        }
        
        /// <summary>
        /// Finds the level manager on a newly loaded scene
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        private IEnumerator FindLevelManager(string sceneName)
        {
            var scene = SceneManager.GetSceneByName(sceneName);
            yield return new WaitUntil(() => scene.isLoaded);
            
            foreach (var go in scene.GetRootGameObjects())
            {
                if (!go.TryGetComponent(out LevelManager levelManager))
                    continue;
                _currentLevelManager = levelManager;
                break;
            }
        }

        /// <summary>
        /// Waits till local player is not null and then sets it up via the level manager
        /// </summary>
        /// <returns></returns>
        private IEnumerator WaitForPlayerToSpawnAndSetItUp()
        {
            var localPlayer = networkManager.SpawnManager.GetLocalPlayerObject();
            if (!localPlayer)
            {
                this.LogError($"{nameof(localPlayer)} is null!" +
                              $"\nDisconnecting!");
                Disconnect();
                yield break;
            }
            yield return _currentLevelManager.SetupLevelAsClient(localPlayer);
        }

        /// <summary>
        /// Disconnect the client from the game.
        /// </summary>
        private void Disconnect()
        {
            networkManager.Shutdown();
        }

        /// <summary>
        /// Quits the game
        /// </summary>
        private static void QuitGame()
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
            {
                UnityEditor.EditorApplication.isPlaying = false;
                return;
            }
#endif
            Application.Quit();
        }
        
        [Serializable]
        private struct LevelWithId
        {
            [field: SerializeField] public Id ID { get; set; }
            [field: SerializeField] public SceneContainer Container { get; set; }
        }
    }
}
