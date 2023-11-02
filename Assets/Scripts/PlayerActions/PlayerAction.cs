using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INTENT
{
    public abstract class PlayerAction : MonoBehaviour
    {
        public abstract void PerformAction();
        public event Action OnActionFinished;
        protected void SuccessFinishAction()
        {
            OnActionFinished?.Invoke();
            OnActionFinished = null;
            Debug.Log("Action finished");
        }
    }
}
