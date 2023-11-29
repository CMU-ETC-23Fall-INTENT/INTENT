using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INTENT
{
    public class ActionTV : PlayerAction
    {
        [SerializeField] private GameObject tvScreen;
        private void OnEnable() 
        {
            GameManager.Instance.PlayerEnterAction();
            tvScreen.SetActive(!tvScreen.activeSelf);
            StartCoroutine(DelayBeforePerformAction(0.5f));
        }
        public override void PerformAction()
        {
            Debug.Log("TV Action");
            GameManager.Instance.PlayerExitAction();
            SuccessFinishAction();
        }
        public override void ResetAction()
        {
            Debug.Log("TV Reset");
        }
        IEnumerator DelayBeforePerformAction(float sec)
        {
            this.enabled = false;
            yield return new WaitForSeconds(sec);
            
            PerformAction();
        }
    }
}
