using UnityEngine;
using UnityEngine.SceneManagement;

namespace PaperDream
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private int _nextSceneIndex;

        public void LoadNextLevel()
        {
            //StartCoroutine(LoadScene(_nextSceneIndex));
            SceneManager.LoadScene(_nextSceneIndex, LoadSceneMode.Single);
        }

        private System.Collections.IEnumerator LoadScene(int index = 0)
        {
            Scene currentScene = SceneManager.GetActiveScene();

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);

            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            yield return SceneManager.UnloadSceneAsync(currentScene);

            SceneManager.SetActiveScene(SceneManager.GetSceneAt(1));
        }
    }
}
