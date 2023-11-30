using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace INTENT
{
    public class ActionPlayerSits : PlayerAction
    {
        [SerializeField] private GameObject chair;
        [SerializeField] private Transform chairTargetTransform;
        [SerializeField] private Transform sitTargetTransform;
        [SerializeField] private float reachedDistance = 0.1f;
        private Vector3 chairStartPosition;
        private GameObject player;
        private NavMeshAgent playerNavMeshAgent;

        private void Awake() 
        {
            chairStartPosition = chair.transform.position;
            player = GameManager.Instance.GetPlayer();
            playerNavMeshAgent = player.GetComponent<NavMeshAgent>();
        }
        public override void StartAction()
        {
            GameManager.Instance.PlayerEnterAction();            
            playerNavMeshAgent.destination = sitTargetTransform.position;
            StartCoroutine(CheckIfReached());
        }
        public override void ResetAction(int state)
        {
            chair.transform.position = chairStartPosition;
        }
        public override void PerformAction()
        {
            this.enabled = false;
            GameManager.Instance.PlayerExitAction();
            
            SuccessFinishAction();
        }
        IEnumerator CheckIfReached()
        {
            while(Vector3.Distance(player.transform.position, sitTargetTransform.position) > reachedDistance)
            {
                yield return null;
            }
            playerNavMeshAgent.destination = player.transform.position;
            StartCoroutine(SitInChair(0.75f));
        }
        IEnumerator SitInChair(float sec)
        {
            float timer = 0f;
            while(timer <= sec)
            {
                chair.transform.position = Vector3.Lerp(chair.transform.position, chairTargetTransform.position, timer / sec);
                chair.transform.rotation = Quaternion.Lerp(chair.transform.rotation, chairTargetTransform.rotation, timer / sec);
                player.transform.position = Vector3.Lerp(player.transform.position, sitTargetTransform.position, timer / sec);
                player.transform.rotation = Quaternion.Lerp(player.transform.rotation, sitTargetTransform.rotation, timer / sec);
                timer += Time.deltaTime;
                yield return null;
            }
            chair.transform.position = chairTargetTransform.position;
            chair.transform.rotation = chairTargetTransform.rotation;
            player.transform.position = sitTargetTransform.position;
            player.transform.rotation = sitTargetTransform.rotation;
            playerNavMeshAgent.destination = player.transform.position;
            StartCoroutine(DelayBeforePerformAction(0.5f));
        }
        IEnumerator DelayBeforePerformAction(float sec)
        {
            yield return new WaitForSeconds(sec);
            PerformAction();
        }
    }
}
