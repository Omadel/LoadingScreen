using UnityEngine;

namespace Etienne.LoadingScreen
{
    [RequireComponent(typeof(TMPro.TextMeshProUGUI))]
    public class LoadingAnimatedText : MonoBehaviour
    {
        [SerializeField] private float frameDuration = .4f;
        [SerializeField] private string[] textFrames = new string[] { "Loading", "Loading .", "Loading ..", "Loading ..." };
        private int frameIndex = 0;
        private TMPro.TextMeshProUGUI textMesh;
        private Timer timer;

        private void Awake()
        {
            textMesh = GetComponent<TMPro.TextMeshProUGUI>();
        }

        private void Start()
        {
            timer = Timer.Create(frameDuration, false).OnComplete(UpdateText);
            timer.Restart();
        }

        private void UpdateText()
        {
            timer.Restart();
            textMesh.text = textFrames[frameIndex];
            frameIndex++;
            frameIndex %= textFrames.Length;
        }
    }
}
