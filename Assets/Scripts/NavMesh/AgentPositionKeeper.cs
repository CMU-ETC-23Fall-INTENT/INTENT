using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INTENT
{
    [RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
    public class AgentPositionKeeper : MonoBehaviour
    {
        private UnityEngine.AI.NavMeshAgent _navMeshAgent;
        private Vector3 _positionToKeep;
        [SerializeField] private float timeToKeepPosition = 3.0f;
        [SerializeField] private float distanceThreshold = 0.1f;

        private void Awake()
        {
            _navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            _positionToKeep = transform.position;
        }
        public void SetPositionToKeep(Vector3 position)
        {
            _positionToKeep = position;
        }

        private void OnEnable()
        {
            StartCoroutine(KeepPosition());
        }
        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private IEnumerator KeepPosition()
        {
            while (true)
            {
                Vector3 deltaPos = transform.position - _positionToKeep;
                if (deltaPos.sqrMagnitude>distanceThreshold)
                {
                    _navMeshAgent.SetDestination(_positionToKeep);
                }
                yield return new WaitForSeconds(timeToKeepPosition);
            }
        }
    }
}
