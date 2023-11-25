using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;

namespace INTENT
{
    public class ActionCoffeeMaking : PlayerAction
    {
        [SerializeField] private CoffeeMachine coffeeMachine;
        [SerializeField] private CoffeeBeans coffeeBeans;
        [SerializeField] private CinemachineVirtualCamera coffeeCamera;
        private void OnEnable() 
        {
            Camera.main.GetComponent<PhysicsRaycaster>().enabled = true;
            coffeeMachine.enabled = true;
            coffeeBeans.enabled = true;
            GameManager.Instance.PlayerEnterAction();
            coffeeCamera.Priority = 11;
        }
        public override void ResetAction()
        {
            IsAvailable = true;
            coffeeMachine.ResetMachine();
            coffeeBeans.ResetBean();
            coffeeCamera.Priority = 9;
        }
        public override void PerformAction()
        {
            IsAvailable = false;
            Camera.main.GetComponent<PhysicsRaycaster>().enabled = false;
            this.enabled = false;
            coffeeMachine.enabled = false;
            GameManager.Instance.PlayerExitAction();
            coffeeCamera.Priority = 9;
            SuccessFinishAction();
        }
    }
}
