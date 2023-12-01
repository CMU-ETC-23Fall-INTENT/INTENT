using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;
using Yarn.Unity;



namespace INTENT
{
    public class ActionBlinds : PlayerAction
    {
        [SerializeField] private CinemachineVirtualCamera blindsCamera;
        [SerializeField] private int closeDegree;
        [SerializeField] private BlindHandle[] blindHandles;
        private int blindCount = 0;
        public override void StartAction() 
        {
            Camera.main.GetComponent<PhysicsRaycaster>().enabled = true;
            GameManager.Instance.PlayerEnterAction();
            blindsCamera.Priority = 11;
            foreach(BlindHandle blindHandle in blindHandles)
            {
                blindHandle.ToggleTargetClose(closeDegree);
                blindHandle.EnableIndicators(false, true);
            }
        }
        public override void ResetAction(int state)
        {
            blindCount = 0;
            blindsCamera.Priority = 9;
            closeDegree = 0;
            foreach(BlindHandle blindHandle in blindHandles)
            {
                blindHandle.FullCloseBlinds(false);
            }
        }
        public void CloseBlind()
        {
            blindCount++;
            if(blindCount == 3)
            {
                StartCoroutine(DelayBeforePerformAction());
            }
        }
        [YarnCommand("NPCToggleBlinds")]
        public void NPCToggleBlinds(bool close)
        {
            foreach(BlindHandle handle in blindHandles)
            {
                handle.FullCloseBlinds(close);
            }
        }
        [YarnCommand("ToggleFullClose")]
        public void ToggleFullClose(int close)
        {
            closeDegree = close;
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
