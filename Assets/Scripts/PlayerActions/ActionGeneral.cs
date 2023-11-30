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

        public override void StartAction()
        {
            GameManager.Instance.PlayerEnterAction();
            StartCoroutine(DelayBeforePerformAction(0.1f));
        }
        public override void ResetAction(int state)
        {
        }
        public override void PerformAction()
        {
            GameManager.Instance.PlayerExitAction();
            SuccessFinishAction();
            actions?.Invoke();
        }
        IEnumerator DelayBeforePerformAction(float sec)
        {
            yield return new WaitForSeconds(sec);
            PerformAction();
        }
    }
}
