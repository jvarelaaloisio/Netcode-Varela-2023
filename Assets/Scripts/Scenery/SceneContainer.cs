using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Scenery
{
    /// <summary>
    /// This object contains logic for scene control.
    /// <p>Initialize with <see cref="Config"/> before usage.</p>
    /// </summary>
    [Serializable]
    public class SceneContainer
    {
        [Header("Config")]
#if UNITY_EDITOR
        [Tooltip("This scene is used to populate the scene name, only when scene name is empty")]
        [SerializeField] private SceneAsset scene;
#endif
        [Tooltip("This field is auto-filled when placing a scene reference.")]
        [SerializeField] private string name = string.Empty;
        [SerializeField] private LoadSceneMode loadMode = LoadSceneMode.Additive;
        private ISceneManager _manager;

        /// <summary>
        /// This scene's name
        /// </summary>
        public string Name
        {
            get => name;
            set => name = value;
        }

        /// <summary>
        /// Initialize this object. Must be called before usage.
        /// </summary>
        /// <param name="manager"></param>
        public void Config(ISceneManager manager)
        {
            _manager = manager;
        }

        /// <summary>
        /// Validates values in this object
        /// </summary>
        public void Validate()
        {
#if UNITY_EDITOR
            if (string.IsNullOrEmpty(name) && scene) name = scene.name;
#endif
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
            if (_manager.GetSceneByName(Name).IsValid())
                _manager.UnloadScene(Name);
            else
                Debug.LogError($"{nameof(SceneContainer)}: Scene [{name}] is not valid");
        }
    }
}