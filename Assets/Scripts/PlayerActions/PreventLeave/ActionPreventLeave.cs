using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace INTENT
{
    public class ActionPreventLeave : PlayerAction
    {
        [SerializeField] Transform playerTarget;
        [SerializeField] string dialogueTitle;
        [SerializeField] string dontLeaveReason;
        [SerializeField] private float reachedDistance = 0.1f;
        private GameObject player;
        private NavMeshAgent playerNavMeshAgent;

        private void Awake() 
        {
            player = GameManager.Instance.GetPlayer();
            playerNavMeshAgent = player.GetComponent<NavMeshAgent>();
        }
        public override void StartAction()
        {
            GameManager.Instance.PlayerEnterAction();
            playerNavMeshAgent.destination = player.transform.position;
            GameManager.Instance.GetDialogueRunner().VariableStorage?.SetValue("$Dont_Leave_Reason", dontLeaveReason);
            GameManager.Instance.GetDialogueRunner().StartDialogue(dialogueTitle);
            GameManager.Instance.GetDialogueRunner().onDialogueComplete.AddListener(MovePlayerToTarget);
        }
        public void MovePlayerToTarget()
        {
            GameManager.Instance.GetDialogueRunner().onDialogueComplete.RemoveAllListeners();
            playerNavMeshAgent.destination = playerTarget.position;
            StartCoroutine(CheckIfReached());
        }
        IEnumerator CheckIfReached()
        {
            while(Vector3.Distance(player.transform.position, playerTarget.position) > reachedDistance)
            {
                yield return null;
            }
            playerNavMeshAgent.destination = player.transform.position;
            PerformAction();
        }
        public override void ResetAction(int state)
        {
        }
        public override void PerformAction()
        {
            GameManager.Instance.PlayerExitAction();
            
            SuccessFinishAction();
        }
    }
}
