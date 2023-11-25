using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;
using Yarn.Unity;
using System;

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
        public override void ResetAction()
        {
            projector.ResetProjector(ActionState);
            cable.ResetCable();
            projectorCamera.Priority = 9;
            IsAvailable = true;
        }
        public override void PerformAction()
        {
            IsAvailable = false;
            ActionState = 0;
            if(projector.Finished)
            {   
                ActionState = 1;
                Camera.main.GetComponent<PhysicsRaycaster>().enabled = false;
                Camera.main.cullingMask |= (1 << LayerMask.NameToLayer("CharacterInvisibleInUI"));
                GameManager.Instance.PlayerExitAction();
                projectorCamera.Priority = 9;
            }
            this.enabled = false;
            projector.enabled = false;            
            SuccessFinishAction();
        }
    }
}
