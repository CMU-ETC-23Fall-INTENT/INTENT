using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;



namespace INTENT
{
    public class ActionBlinds : PlayerAction
    {
        [SerializeField] private CinemachineVirtualCamera blindsCamera;
        private int blindCount = 0;
        private void OnEnable() 
        {
            Camera.main.GetComponent<PhysicsRaycaster>().enabled = true;
            GameManager.Instance.PlayerEnterAction();
            blindsCamera.Priority = 11;
        }
        public void CloseBlind()
        {
            blindCount++;
            if(blindCount == 3)
            {
                StartCoroutine(DelayBeforePerformAction());
            }
        }
        IEnumerator DelayBeforePerformAction()
        {
            yield return new WaitForSeconds(0.5f);
            PerformAction();
        }
        public override void PerformAction()
        {
            Camera.main.GetComponent<PhysicsRaycaster>().enabled = false;
            GameManager.Instance.PlayerExitAction();
            blindsCamera.Priority = 9;
            SuccessFinishAction();
        }
    }
}
