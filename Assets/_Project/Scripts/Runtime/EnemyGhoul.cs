using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

namespace Etienne.LoadingScreen
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyGhoul : MonoBehaviour
    {
        [SerializeField, Etienne.MinMaxRange(0, 30)] private Etienne.Range distanceRange = new Range(15f);
        [SerializeField, Etienne.MinMaxRange(0, 30)] private Etienne.Range speedRange = new Range(3.5f, 15f);
        [SerializeField] private AnimationCurve speedCurve;
        private NavMeshAgent agent;
        private Transform playerTransform;
        private new Camera camera;
        private Animator animator;
        private Rig rig;
        private int idleAnimation = Animator.StringToHash("Crouching Idle");
        private int walkAnimation = Animator.StringToHash("Crawl");


        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.acceleration = 1000f;
            agent.angularSpeed = 1000f;
            animator = GetComponentInChildren<Animator>();
            rig = GetComponentInChildren<Rig>();
        }

        private void Start()
        {
            camera = Camera.main;
        }

        private void Update()
        {
            if (camera == null)
            {
                camera = Camera.main;
                playerTransform = camera.transform.root;
            }
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(.5f, .6f));
            if (!Physics.Raycast(ray, out RaycastHit hit)) return;

            NavMesh.SamplePosition(hit.point, out NavMeshHit navhit, 1000, NavMesh.AllAreas);

            if (agent.path.corners != null)
            {
                float remainingDistance = GetPathDistance(agent.path.corners);
                float normalizedDistance = distanceRange.Normalize(remainingDistance);
                float evaluatedCurve = speedCurve.Evaluate(normalizedDistance);
                agent.speed = speedRange.Lerp(evaluatedCurve);
            }

            float distance = Vector3.Distance(transform.position, navhit.position);
            agent.isStopped = distance <= .2f;
            if (agent.isStopped)
            {
                rig.weight = Mathf.Lerp(rig.weight, 1, Time.deltaTime * 5);
                Vector3 direction = playerTransform.position - transform.position;
                direction.Normalize();
                transform.forward = Vector3.Slerp(transform.forward, direction, Time.deltaTime * 5);
                animator.Play(idleAnimation);
            }
            else
            {
                rig.weight = Mathf.Lerp(rig.weight, 0, Time.deltaTime * 5);
                agent.SetDestination(navhit.position);
                animator.Play(walkAnimation);
                animator.SetFloat("Speed", speedRange.Normalize(agent.speed));
            }
        }

        private float GetPathDistance(Vector3[] waypoints)
        {
            float lenght = 0f;
            for (int i = 1; i < waypoints.Length; i++)
            {
                lenght += Vector3.Distance(waypoints[i - 1], waypoints[i]);
            }
            return lenght;
        }
    }
}
