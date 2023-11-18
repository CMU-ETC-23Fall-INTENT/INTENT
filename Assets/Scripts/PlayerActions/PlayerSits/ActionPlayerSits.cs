using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INTENT
{
    public class ActionPlayerSits : PlayerAction
    {
        [SerializeField] private GameObject chair;
        [SerializeField] private Transform chairTargetTransform;
        [SerializeField] private Transform sitTargetTransform;
        private GameObject player;

        private void Awake() 
        {
            player = GameManager.Instance.GetPlayer();
        }
        private void OnEnable()
        {
            StartCoroutine(MoveChair(1f));
            
            player.GetComponent<UnityEngine.AI.NavMeshAgent>().destination = sitTargetTransform.position;
            player.GetComponent<UnityEngine.AI.NavMeshAgent>().Warp(sitTargetTransform.position);
            player.transform.position = sitTargetTransform.position;
            player.transform.rotation = sitTargetTransform.rotation;
            StartCoroutine(DelayBeforePerformAction(1f));
        }
        public override void PerformAction()
        {
            SuccessFinishAction();
        }
        IEnumerator MoveChair(float sec)
        {
            float timer = 0f;
            while(timer <= sec)
            {
                chair.transform.position = Vector3.Lerp(chair.transform.position, chairTargetTransform.position, timer / sec);
                chair.transform.rotation = Quaternion.Lerp(chair.transform.rotation, chairTargetTransform.rotation, timer / sec);
                timer += Time.deltaTime;
                yield return null;
            }
            chair.transform.position = chairTargetTransform.position;
            chair.transform.rotation = chairTargetTransform.rotation;
        }
        IEnumerator DelayBeforePerformAction(float sec)
        {
            yield return new WaitForSeconds(sec);
            PerformAction();
        }
    }
}
