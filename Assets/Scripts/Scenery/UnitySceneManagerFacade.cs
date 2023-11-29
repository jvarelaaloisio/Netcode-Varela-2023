using UnityEngine.SceneManagement;

namespace Scenery
{
    public class UnitySceneManagerFacade : ISceneManager
    {
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