using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INTENT
{
    public class ActionTV : PlayerAction
    {
        [SerializeField] private GameObject tvScreen;
        public override void StartAction()
        {
            GameManager.Instance.PlayerEnterAction();
            switch(tvScreen.activeSelf)
            {
                case false:
                    SoundManager2D.Instance.PlaySFX("TVSwitch");
                    SoundManager2D.Instance.PlaySFX("TVDogCat");
                    break;
                case true:
                    SoundManager2D.Instance.PlaySFX("TVSwitch");
                    SoundManager2D.Instance.StopSFX();
                    break;
            }

            tvScreen.SetActive(!tvScreen.activeSelf);
            StartCoroutine(DelayBeforePerformAction(0.5f));
        }
        public override void PerformAction()
        {
            Debug.Log("TV Action");
            GameManager.Instance.PlayerExitAction();
            SuccessFinishAction();
        }
        public override void ResetAction(int state)
        {
            Debug.Log("TV Reset");
        }
        IEnumerator DelayBeforePerformAction(float sec)
        {
            yield return new WaitForSeconds(sec);
            
            PerformAction();
        }
    }
}
