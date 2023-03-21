using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Etienne.LoadingScreen
{
    internal enum Scenes { MainMenu, LoadingScreen, Level }
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button playButton, quitButton;
        private void Awake()
        {
            playButton.onClick.AddListener(Play);
            quitButton.onClick.AddListener(Quit);
        }

        private void Play()
        {
            Debug.Log("Play");
            StartCoroutine(UnloadMainMenu());
        }

        private IEnumerator UnloadMainMenu()
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            AsyncOperation operation = SceneManager.LoadSceneAsync((int)Scenes.LoadingScreen, new LoadSceneParameters(LoadSceneMode.Additive, LocalPhysicsMode.None));
            yield return new WaitUntil(() => operation.isDone);
            canvas.enabled = false;
            EventSystem.current.gameObject.SetActive(false);
            canvas = FindObjectsOfType<Canvas>()[^1];
            canvas.worldCamera = Camera.main;


            operation = SceneManager.LoadSceneAsync((int)Scenes.Level, LoadSceneMode.Additive);
            operation.allowSceneActivation = false;
            float time = 5f;
            yield return new WaitForSecondsRealtime(time);
            time = FindObjectOfType<MainMenuGhoul>().PlayAnimation();
            yield return new WaitForSecondsRealtime(time);

            Camera main = Camera.main;
            main.GetComponent<AudioListener>().enabled = false;
            operation.allowSceneActivation = true;
            yield return new WaitUntil(() => operation.isDone);
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex((int)Scenes.Level));
            main.enabled = false;
            SceneManager.UnloadSceneAsync((int)Scenes.MainMenu);
            SceneManager.UnloadSceneAsync((int)Scenes.LoadingScreen);
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
