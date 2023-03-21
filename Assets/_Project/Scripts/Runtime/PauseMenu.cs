using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Etienne.LoadingScreen
{
    [RequireComponent(typeof(CanvasGroup))]
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private KeyCode pauseKey = KeyCode.Escape;
        [SerializeField] private Button resumeButton, quitButton;
        private CanvasGroup group;
        private bool isPaused = false;

        private void Awake()
        {
            group = GetComponent<CanvasGroup>();
        }

        private void Start()
        {
            resumeButton.onClick.AddListener(Resume);
            quitButton.onClick.AddListener(Quit);
            Resume();
            tween.Complete();
            group.DOComplete();
        }

        private void Update()
        {
            if (Input.GetKeyDown(pauseKey))
            {
                TogglePause();
            }
        }

        private void TogglePause()
        {
            if (isPaused) Resume();
            else PauseGame();
        }

        private Tween tween;
        private void PauseGame()
        {
            isPaused = true;
            group.DOKill();
            group.DOFade(1f, .4f);
            group.interactable = isPaused;
            group.blocksRaycasts = isPaused;
            tween?.Kill();
            tween = DOTween.To(() => Time.timeScale, v => Time.timeScale = v, .01f, .4f).SetUpdate(true);
        }

        private void Resume()
        {
            isPaused = false;
            group.DOKill();
            group.DOFade(0, .4f);
            group.interactable = isPaused;
            group.blocksRaycasts = isPaused;
            tween?.Kill();
            tween = DOTween.To(() => Time.timeScale, v => Time.timeScale = v, 1f, .4f).SetUpdate(true);
        }

        private void Quit()
        {
            Resume();
            tween.Complete();
            SceneManager.LoadScene(0);
        }
    }
}
