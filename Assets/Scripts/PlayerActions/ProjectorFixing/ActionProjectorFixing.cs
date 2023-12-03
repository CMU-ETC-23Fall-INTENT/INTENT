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
        public override void StartAction()
        {
            Camera.main.GetComponent<PhysicsRaycaster>().enabled = true;
            Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer("CharacterInvisibleInUI") | (1 << LayerMask.NameToLayer("InteractionPoints")));
            GameManager.Instance.PlayerEnterAction();
            projector.enabled = true;
            projectorCamera.Priority = 11;
        }
        public override void ResetAction(int state)
        {
            Debug.Log("Reset Projector Fixing");
            projector.ResetProjector(ActionState);
            
            cable.ResetCable(state == 1);

            projectorCamera.Priority = 9;
        }
        public override void PerformAction()
        {
            ActionState = 0;
            if(projector.Finished)
            {   
                ActionState = 1;
                Camera.main.GetComponent<PhysicsRaycaster>().enabled = false;
                Camera.main.cullingMask |= 1 << LayerMask.NameToLayer("CharacterInvisibleInUI") | (1 << LayerMask.NameToLayer("InteractionPoints"));
                GameManager.Instance.PlayerExitAction();
                projectorCamera.Priority = 9;
            }
            projector.enabled = false;            
            SuccessFinishAction();
        }
    }
}
