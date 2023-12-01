using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INTENT
{
    public abstract class PlayerAction : MonoBehaviour
    {
        public int ActionState = 0;
        public abstract void StartAction();
        public abstract void PerformAction();
        public abstract void ResetAction(int state);
        public event Action OnActionFinished;
        protected void SuccessFinishAction()
        {
            OnActionFinished?.Invoke();
            OnActionFinished = null;
        }
    }
}
