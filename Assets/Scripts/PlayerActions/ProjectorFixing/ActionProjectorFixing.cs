using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;

namespace INTENT
{
    public class ActionProjectorFixing : PlayerAction
    {
        [SerializeField] private CinemachineVirtualCamera projectorCamera;
        private void OnEnable() 
        {
            Camera.main.GetComponent<PhysicsRaycaster>().enabled = true;
            GameManager.Instance.PlayerEnterAction();
            projectorCamera.Priority = 11;
            
        }
        public override void PerformAction()
        {
            
            Camera.main.GetComponent<PhysicsRaycaster>().enabled = false;
            GameManager.Instance.PlayerExitAction();
            projectorCamera.Priority = 9;
            SuccessFinishAction();
        }
    }
}
