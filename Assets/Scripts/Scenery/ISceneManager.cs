using UnityEngine.SceneManagement;

namespace Scenery
{
    public interface ISceneManager
    {
        void LoadScene(string sceneName);
        void LoadScene(string sceneName, LoadSceneMode mode);
        void LoadScene(string sceneName, LoadSceneParameters parameters);
        void UnloadScene(string sceneName);
        Scene GetSceneByName(string name);
    }
}
