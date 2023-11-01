using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INTENT
{
    public class ActionCoffeeMaking : PlayerAction
    {
        private void OnEnable() {
            
        }
        public override void PerformAction()
        {
           Debug.Log("Test action");
        }
    }
}
