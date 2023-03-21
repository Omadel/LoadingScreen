using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Etienne.LoadingScreen
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button playButton, quitButton;
        private void Awake()
        {
            playButton.onClick.AddListener(Play);
            quitButton.onClick.AddListener(Quit);
        }

        //private IEnumerator Start()
        //{
        //loadingScreenOperation = SceneManager.LoadSceneAsync(1, new LoadSceneParameters(LoadSceneMode.Additive, LocalPhysicsMode.None));
        //loadingScreenOperation.allowSceneActivation = false;
        //Debug.Log("start Loading");
        //yield return new WaitUntil(() => loadingScreenOperation.isDone);
        //Debug.Log("Loading completed");
        //}

        private void Play()
        {
            Debug.Log("Play");
            StartCoroutine(UnloadMainMenu());
        }

        private IEnumerator UnloadMainMenu()
        {
            var camera = Camera.main;
            AsyncOperation operation = SceneManager.LoadSceneAsync(1, new LoadSceneParameters(LoadSceneMode.Additive, LocalPhysicsMode.None));
            yield return new WaitUntil(() => operation.isDone);
            camera.gameObject.SetActive(false);


            operation = SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
            operation.allowSceneActivation = false;
            yield return new WaitForSecondsRealtime(10f);
            operation.allowSceneActivation = true;
            yield return new WaitUntil(() => operation.isDone);

            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(2));

            SceneManager.UnloadSceneAsync(1);
            SceneManager.UnloadSceneAsync(0);
        }

        private void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
	Application.Quit();
#endif
        }
    }
}
