using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.UI;
using TMPro;

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
        [SerializeField] private GameObject desktopPage;

        [Header("Ending Email Page")]
        [SerializeField] private TextMeshProUGUI emailTitleText;
        [SerializeField] private TextMeshProUGUI emailMainText;

        [Header("Good Ending Texts")]
        [SerializeField] private string goodEndingTitle;
        
        [TextArea(3, 10)]
        [SerializeField] private string goodEndingMain;

        [Header("Ali Remove Tony Texts")]
        [SerializeField] private string aliRemoveTonyTitle;
        
        [TextArea(3, 10)]
        [SerializeField] private string aliRemoveTonyMain;

        [Header("Tony Leave Texts")]
        [SerializeField] private string tonyLeaveTitle;
        
        [TextArea(3, 10)]
        [SerializeField] private string tonyLeaveMain;




        private GameObject currentPage;
        private void Awake() 
        {
            currentPage = desktopPage;
        }
        public override void StartAction()
        {
            GameManager.Instance.PlayerEnterAction();
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
            UIManager.Instance.FadeIn(1f);
        }
        
        public void OpenPage(GameObject page)
        {
            currentPage.SetActive(false);
            currentPage = page;
            currentPage.SetActive(true);
        }

        [YarnCommand("SetEndingType")]
        public void SetEndingType(int type)
        {
            switch(type)
            {
                case 0:
                    ActionState = 0;
                    endingType = EndingType.Best;
                    emailTitleText.text = goodEndingTitle;
                    emailMainText.text = goodEndingMain;
                    break;
                case 1:
                    ActionState = 1;
                    endingType = EndingType.AliKickTony;
                    emailTitleText.text = aliRemoveTonyTitle;
                    emailMainText.text = aliRemoveTonyMain;
                    break;
                case 2:
                    ActionState = 2;
                    endingType = EndingType.TonyRemoveSelf;
                    emailTitleText.text = tonyLeaveTitle;
                    emailMainText.text = tonyLeaveMain;
                    break;
            }
        }
        public override void PerformAction()
        {
            StartCoroutine(FinishFadeOut(1f));
        }
        public override void ResetAction(int state)
        {
            OpenPage(desktopPage);
            switch(state)
            {
                case 0:
                    endingType = EndingType.Best;
                    break;
                case 1:
                    endingType = EndingType.AliKickTony;
                    break;
                case 2:
                    endingType = EndingType.TonyRemoveSelf;
                    break;
            }
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
            canvasGroup.alpha = 1f;
            OpenPage(desktopPage);
            GameManager.Instance.PlayerExitAction();
            UIManager.Instance.OpenLearnPanel(true);
            SuccessFinishAction();
        }
    }
}
