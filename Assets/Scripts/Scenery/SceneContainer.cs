using System;
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

        public void Unload()
        {
            _manager.UnloadScene(Name);
        }
    }
}