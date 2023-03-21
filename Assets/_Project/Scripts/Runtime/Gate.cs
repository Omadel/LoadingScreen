using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Etienne.LoadingScreen
{
    public class Gate : MonoBehaviour
    {
        [SerializeField] private Transform rightGate, leftGate;
        private NavMeshObstacle obstacle;
        private List<Collider> inside = new List<Collider>();
        private void Awake()
        {
            obstacle = GetComponent<NavMeshObstacle>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (inside.Count <= 0) Open(other);
            inside.Add(other);
        }

        private void Open(Collider other)
        {
            obstacle.enabled = false;
            rightGate.DOKill();
            leftGate.DOKill();
            Vector3 direction = transform.position - other.transform.position;
            direction.Normalize();
            float dot = Vector3.Dot(transform.forward, direction);
            Vector3 openValue = new Vector3(0, 90, 0);
            openValue.y *= dot < 0 ? -1 : 1;
            rightGate.DORotate(openValue, .4f).SetEase(Ease.OutBounce);
            openValue.y *= -1;
            leftGate.DORotate(openValue, .4f).SetEase(Ease.OutBounce);
        }

        private void OnTriggerExit(Collider other)
        {
            inside.Remove(other);
            if (inside.Count <= 0) Close();
        }

        private void Close()
        {
            obstacle.enabled = true;
            rightGate.DOKill();
            leftGate.DOKill();
            Vector3 closeValue = new Vector3(0, 0, 0);
            rightGate.DORotate(closeValue, .4f).SetEase(Ease.OutBounce);
            leftGate.DORotate(closeValue, .4f).SetEase(Ease.OutBounce);
        }
    }
}
