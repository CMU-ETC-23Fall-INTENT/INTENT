using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INTENT
{
    public class ActionRoomSetup : PlayerAction
    {
        [SerializeField] private GameObject tvScreen;
        [SerializeField] private GameObject papers;

        private void OnEnable()
        {
            tvScreen.SetActive(true);
            papers.SetActive(true);
            StartCoroutine(DelayBeforePerformAction(0.5f));
        }
        public override void PerformAction()
        {
            SuccessFinishAction();
        }
        IEnumerator DelayBeforePerformAction(float sec)
        {
            yield return new WaitForSeconds(sec);
            PerformAction();
        }
    }
}
