using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INTENT
{
    public class ActionRoomSetup : PlayerAction
    {
        [SerializeField] private GameObject tvScreen;
        [SerializeField] private GameObject papers;

        public override void StartAction()
        {
            GameManager.Instance.PlayerEnterAction();
            tvScreen.SetActive(true);
            papers.SetActive(true);
            StartCoroutine(DelayBeforePerformAction(0.5f));
        }
        public override void ResetAction(int state)
        {
            switch(state)
            {
                case 1:
                    tvScreen.SetActive(true);
                    papers.SetActive(true);
                    break;
                case 0:
                    tvScreen.SetActive(false);
                    papers.SetActive(false);
                    break;
            }
        }
        public override void PerformAction()
        {
            ActionState = 1;
            GameManager.Instance.PlayerExitAction();
            SuccessFinishAction();
        }
        IEnumerator DelayBeforePerformAction(float sec)
        {
            yield return new WaitForSeconds(sec);
            PerformAction();
        }
    }
}
