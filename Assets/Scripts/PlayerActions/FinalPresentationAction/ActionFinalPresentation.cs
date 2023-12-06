using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INTENT
{
    public class ActionFinalPresentation : PlayerAction
    {
        public override void StartAction()
        {
            GameManager.Instance.PlayerEnterAction();
        }
        public override void ResetAction(int state)
        {
            Debug.Log("Reset Final Presentation");
        }
        public override void PerformAction()
        {
            GameManager.Instance.PlayerExitAction();
            SuccessFinishAction();
        }
    }
}
