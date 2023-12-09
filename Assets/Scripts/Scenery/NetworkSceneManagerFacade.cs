using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenery
{
    public class NetworkSceneManagerFacade : ISceneManager
    {
        private readonly NetworkSceneManager _networkSceneManager;

        public NetworkSceneManagerFacade(NetworkSceneManager networkSceneManager)
        {
            _networkSceneManager = networkSceneManager;
        }

        public void LoadScene(string sceneName, LoadSceneMode mode)
            => _networkSceneManager.LoadScene(sceneName, mode);

        public void LoadScene(string sceneName)
            => LoadScene(sceneName, LoadSceneMode.Additive);

        public void LoadScene(string sceneName, LoadSceneParameters parameters)
            => LoadScene(sceneName, parameters.loadSceneMode);

        public void UnloadScene(string sceneName)
        {
            var scene = GetSceneByName(sceneName);
            if (!scene.IsValid())
            {
                Debug.LogError($"{nameof(NetworkSceneManagerFacade)}: Scene ({sceneName}) is not valid!");
                return;
            }

            if (!scene.isLoaded)
            {
                Debug.LogError($"{nameof(NetworkSceneManagerFacade)}: Scene ({sceneName}) is not loaded!");
                return;
            }

            if (scene.IsValid() && scene.isLoaded) _networkSceneManager.UnloadScene(scene);
        }

        public Scene GetSceneByName(string name)
            => SceneManager.GetSceneByName(name);

        public IEnumerator LoadSceneAsync(string sceneName, LoadSceneMode _)
        {
            bool isLoaded = false;
            LoadScene(sceneName);
            _networkSceneManager.OnLoadEventCompleted += HandleOnLoad;
            yield return new WaitUntil(() => isLoaded);
            
            void HandleOnLoad(string newlyLoadedSceneName, LoadSceneMode loadscenemode, List<ulong> clientscompleted, List<ulong> clientstimedout)
            {
                if (newlyLoadedSceneName == sceneName)
                    isLoaded = true;
            }
        }
    }
}