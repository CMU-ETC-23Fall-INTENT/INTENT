using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INTENT
{
    public abstract class PlayerAction : MonoBehaviour
    {
        public int ActionState = 0;
        public bool IsAvailable = true;
        public abstract void PerformAction();
        public abstract void ResetAction();
        public event Action OnActionFinished;
        protected void SuccessFinishAction()
        {
            OnActionFinished?.Invoke();
            OnActionFinished = null;
        }
    }
}
