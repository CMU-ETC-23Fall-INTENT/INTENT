using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INTENT
{
    public class ActionGoHome : PlayerAction
    {
        private void OnEnable() 
        {
            GameManager.Instance.PlayerEnterAction();
            StartCoroutine(DelaySwitchEpisode(1f));
            StartCoroutine(DelayBeforePerformAction(0.5f));
        }
        public override void ResetAction()
        {
            IsAvailable = true;
        }
        public override void PerformAction()
        {
            SuccessFinishAction();
            IsAvailable = false;
            GameManager.Instance.PlayerExitAction();
        }
        
        IEnumerator DelayBeforePerformAction(float sec)
        {
            yield return new WaitForSeconds(sec);
            PerformAction();
        }
        IEnumerator DelaySwitchEpisode(float sec)
        {
            yield return new WaitForSeconds(sec);
            TaskManager.Instance.ActivateEpisode(Episode.Episode2);
        }
        
    }
}
