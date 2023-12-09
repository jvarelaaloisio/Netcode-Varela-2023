using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenery
{
    [Serializable]
    public class SceneContainer
    {
        [SerializeField] private string name = string.Empty;
        [SerializeField] private LoadSceneMode loadMode = LoadSceneMode.Additive;
        private ISceneManager _manager;

        public string Name
        {
            get => name;
            set => name = value;
        }

        public void Init(ISceneManager manager)
        {
            _manager = manager;
        }

        public void Load()
        {
            _manager.LoadScene(Name, loadMode);
        }

        /// <summary>
        /// Loads the scene asynchronously
        /// </summary>
        /// <returns></returns>
        public IEnumerator LoadAsync()
        {
            yield return _manager.LoadSceneAsync(name, loadMode);
        }
        
        public void Unload()
        {
            _manager.UnloadScene(Name);
        }
    }
}