using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.UI;

namespace INTENT
{
    public enum EndingType
    {
        Good,
        Mid,
        Bad
    }
    public class ActionFinalEmail : PlayerAction
    {
        private EndingType endingType;
        [SerializeField] private Sprite goodEndingSprite;
        [SerializeField] private Sprite midEndingSprite;
        [SerializeField] private Sprite badEndingSprite;
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
                    endingType = EndingType.Good;
                    endingImage.sprite = goodEndingSprite;
                    break;
                case 1:
                    endingType = EndingType.Mid;
                    endingImage.sprite = midEndingSprite;
                    break;
                case 2:
                    endingType = EndingType.Bad;
                    endingImage.sprite = badEndingSprite;
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
