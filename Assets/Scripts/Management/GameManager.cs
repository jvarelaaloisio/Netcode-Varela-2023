using System.Collections;
using UnityEngine;
using Unity.Netcode;

using Events.Runtime.Channels;
using Events.Runtime.Channels.Helpers;
using Scenery;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

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

        [Header("Events")]
        public UnityEvent onHost;
        public UnityEvent onJoin;
        public UnityEvent onDisconnect;
        
        private readonly ISceneManager _unitySceneManager = new UnitySceneManagerFacade();
        private ISceneManager _sceneManager;

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
            disconnectChannel.TrySubscribe(ShutDownConnection);
        }

        private void OnDisable()
        {
            hostChannel.TryUnsubscribe(HostGame);
            joinChannel.TryUnsubscribe(JoinGame);
            disconnectChannel.TryUnsubscribe(ShutDownConnection);
        }

        /// <summary>
        /// Starts Game as Host
        /// </summary>
        private void HostGame()
        {
            networkManager.StartHost();
            StartGame();

            var levelScene = SceneManager.GetSceneByName(level.Name);
            StartCoroutine(InitializeLevel(levelScene));
            onHost.Invoke();
        }

        /// <summary>
        /// Connects to game as client
        /// </summary>
        private void JoinGame()
        {
            networkManager.StartClient();
            StartGame();
            onJoin.Invoke();
        }

        /// <summary>
        /// Disconnect the client from the game.
        /// </summary>
        private void ShutDownConnection()
        {
            networkManager.Shutdown();
        }

        /// <summary>
        /// Loads level 1 and subscribes to <see cref="NetworkManager.OnClientStopped"/>
        /// </summary>
        private void StartGame()
        {
            level.Init(new UnitySceneManagerFacade());
            level.Load();
            networkManager.OnClientStopped += HandleClientStopped;
        }

        private void HandleClientStopped(bool wasHost)
        {
            level.Unload();
            networkManager.OnClientStopped -= HandleClientStopped;
            onDisconnect.Invoke();
        }

        private static IEnumerator InitializeLevel(Scene scene)
        {
            yield return new WaitUntil(() => scene.isLoaded);
            
            foreach (var go in scene.GetRootGameObjects())
            {
                if (!go.TryGetComponent(out LevelManager levelManager))
                    continue;
                yield return levelManager.Init();
                break;
            }
        }
    }
}
