using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenery
{
    public class UnitySceneManagerFacade : ISceneManager
    {
        public IEnumerator LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode)
        {
            var loadOperation = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
            yield return new WaitUntil(() => loadOperation.isDone);
        }
        public void LoadScene(string sceneName)
            => SceneManager.LoadScene(sceneName);

        public void LoadScene(string sceneName, LoadSceneMode mode)
            => SceneManager.LoadScene(sceneName, mode);

        public void LoadScene(string sceneName, LoadSceneParameters parameters)
            => SceneManager.LoadScene(sceneName, parameters);

        public void UnloadScene(string sceneName)
            => SceneManager.UnloadSceneAsync(sceneName);

        public Scene GetSceneByName(string name)
            => SceneManager.GetSceneByName(name);
    }
}