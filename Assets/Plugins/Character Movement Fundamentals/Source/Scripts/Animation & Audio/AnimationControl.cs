using UnityEngine;

namespace CMF
{
    //This script controls the character's animation by passing velocity values and other information ('isGrounded') to an animator component;
    public class AnimationControl : MonoBehaviour
    {
        private Controller controller;
        private Animator animator;
        private int idleAnimation = Animator.StringToHash("Idle");
        private int walkAnimation = Animator.StringToHash("Walk");
        private int runAnimation = Animator.StringToHash("Run");
        private int jumpAnimation = Animator.StringToHash("Jump");


        //Setup;
        private void Awake()
        {
            controller = GetComponent<Controller>();
            animator = GetComponentInChildren<Animator>();
        }

        //OnEnable;
        private void OnEnable()
        {
            //Connect events to controller events;
            controller.OnLand += OnLand;
            controller.OnJump += OnJump;
        }

        //OnDisable;
        private void OnDisable()
        {
            //Disconnect events to prevent calls to disabled gameobjects;
            controller.OnLand -= OnLand;
            controller.OnJump -= OnJump;
        }

        //Update;
        private void Update()
        {
            if (!controller.IsGrounded()) return;
            //Get controller velocity;
            Vector3 _velocity = controller.GetVelocity();
            if (_velocity.sqrMagnitude > 0)
            {
                animator.Play(controller.IsSprinting() ? runAnimation : walkAnimation);

            }
            else
            {
                animator.Play(idleAnimation);

            }
        }

        private void OnLand(Vector3 _v)
        {
            //animator.SetTrigger("OnLand");
        }

        private void OnJump(Vector3 _v)
        {
            animator.Play(jumpAnimation);
        }
    }
}
