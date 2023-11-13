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
        [SerializeField] private Projector projector;
        [SerializeField] private Cable cable;
        private void OnEnable() 
        {
            Camera.main.GetComponent<PhysicsRaycaster>().enabled = true;
            Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer("CharacterInvisibleInUI"));
            GameManager.Instance.PlayerEnterAction();
            projector.enabled = true;
            projectorCamera.Priority = 11;
            
        }
        public override void PerformAction()
        {
            
            Camera.main.GetComponent<PhysicsRaycaster>().enabled = false;
            Camera.main.cullingMask |= (1 << LayerMask.NameToLayer("CharacterInvisibleInUI"));
            this.enabled = false;
            projector.enabled = false;
            GameManager.Instance.PlayerExitAction();
            projectorCamera.Priority = 9;
            SuccessFinishAction();
        }
    }
}
