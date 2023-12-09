using System;
using System.Collections;
using Core.Extensions;
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
        [SerializeField] private SceneContainer level;
        
        [Header("Channels Listened")]
        [SerializeField] private VoidChannelSo hostChannel;
        [SerializeField] private VoidChannelSo joinChannel;
        [SerializeField] private VoidChannelSo disconnectChannel;
        [SerializeField] private VoidChannelSo quitChannel;

        [Header("Events")]
        public UnityEvent onHost;
        public UnityEvent onJoin;
        public UnityEvent onDisconnect;
        
        private readonly ISceneManager _unitySceneManager = new UnitySceneManagerFacade();
        private ISceneManager _sceneManager;
        private LevelManager _currentLevelManager;

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

            mainMenu.Init(_unitySceneManager);
            mainMenu.Load();
        }

        private void OnEnable()
        {
            hostChannel.TrySubscribe(HostGame);
            joinChannel.TrySubscribe(JoinGame);
            disconnectChannel.TrySubscribe(Disconnect);
            quitChannel.TrySubscribe(QuitGame);
        }

        private void OnDisable()
        {
            hostChannel.TryUnsubscribe(HostGame);
            joinChannel.TryUnsubscribe(JoinGame);
            disconnectChannel.TryUnsubscribe(Disconnect);
            quitChannel.TryUnsubscribe(QuitGame);
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

            yield return StartGame(onHost.Invoke);

            yield return FindLevelManager(level.Name);
            
            yield return _currentLevelManager.InitAsHost();
            
            yield return WaitForPlayerToSpawnAndSetItUp();
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
            
            yield return StartGame(onJoin.Invoke);
            
            yield return FindLevelManager(level.Name);
            
            yield return new WaitUntil(() => networkManager.LocalClient != null);
            
            yield return new WaitUntil(() => networkManager.LocalClient.PlayerObject != null);
            
            yield return WaitForPlayerToSpawnAndSetItUp();
        }

        /// <summary>
        /// Loads level 1 and subscribes to <see cref="NetworkManager.OnClientStopped"/>
        /// </summary>
        /// <param name="onFinish"></param>
        private IEnumerator StartGame(Action onFinish)
        {
            level.Init(new UnitySceneManagerFacade());
            yield return level.LoadAsync();
            networkManager.OnClientStopped += HandleClientStopped;
            onFinish?.Invoke();
        }

        private void HandleClientStopped(bool wasHost)
        {
            level.Unload();
            networkManager.OnClientStopped -= HandleClientStopped;
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
            yield return _currentLevelManager.SetupPlayer(localPlayer);
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
    }
}
