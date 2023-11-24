using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.UI;

namespace INTENT
{
    public enum EndingType
    {
        Best,
        AliKickTony,
        TonyRemoveSelf
    }
    public class ActionFinalEmail : PlayerAction
    {
        private EndingType endingType;
        [SerializeField] private Sprite bestEndingSprite;
        [SerializeField] private Sprite aliKickTonySprite;
        [SerializeField] private Sprite tonyRemoveSelfSprite;
        [SerializeField] private Image endingImage;

        private void OnEnable() 
        {
            GameManager.Instance.PlayerEnterAction();
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
            UIManager.Instance.FadeIn(1f);
        }

        [YarnCommand("SetEndingType")]
        public void SetEndingType(int type)
        {
            switch(type)
            {
                case 0:
                    endingType = EndingType.Best;
                    endingImage.sprite = bestEndingSprite;
                    break;
                case 1:
                    endingType = EndingType.AliKickTony;
                    endingImage.sprite = aliKickTonySprite;
                    break;
                case 2:
                    endingType = EndingType.TonyRemoveSelf;
                    endingImage.sprite = tonyRemoveSelfSprite;
                    break;
            }
        }
        public override void PerformAction()
        {
            StartCoroutine(FinishFadeOut(1f));
        }
        IEnumerator FinishFadeOut(float sec)
        {
            float timer = 0f;
            CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
            while(timer < sec)
            {
                timer += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / sec);
                yield return null;
            }
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
            this.enabled = false;
            GameManager.Instance.PlayerExitAction();
            UIManager.Instance.OpenLearnPanel(true);
            SuccessFinishAction();
        }
    }
}
