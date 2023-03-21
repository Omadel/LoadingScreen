using DG.Tweening;
using UnityEngine;

namespace Etienne.LoadingScreen
{
    [RequireComponent(typeof(Animator), typeof(Path))]
    public class MainMenuGhoul : MonoBehaviour
    {
        [SerializeField] private float duration = 2f;
        [SerializeField] private AnimationCurve ease;
        private Path path;
        private Animator animator;
        private Tween tween;
        private void Awake()
        {
            animator = GetComponent<Animator>();
            path = GetComponent<Path>();
        }

        public float PlayAnimation()
        {
            tween = transform.DOPath(path.WorldWaypoints, duration, PathType.CatmullRom, PathMode.Full3D, 2)
                .SetEase(ease)
                .OnUpdate(UpdateAnimatorSpeed);
            animator.CrossFadeInFixedTime("Crawl", .4f, 0);
            return duration;
        }

        private void UpdateAnimatorSpeed()
        {
            float time = tween.ElapsedPercentage();
            animator.SetFloat("Speed", ease.Evaluate(time));
            if (time < 1f)
            {
                Vector3 direction = tween.PathGetPoint(time + Time.deltaTime) - transform.position;
                direction.Normalize();
                transform.forward = Vector3.Slerp(transform.forward, direction, .2f);
            }
        }
    }
}
