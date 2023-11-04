using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace INTENT
{
    public class ActionGeneral : PlayerAction
    {
        //[SerializeField] public event Action actions;
        [SerializeField] public UnityEvent actions;

        private void OnEnable()
        {
            PerformAction();
        }
        public override void PerformAction()
        {
            Debug.Log("ActionGeneral: Performing action");
            actions?.Invoke();
        }
    }
}
