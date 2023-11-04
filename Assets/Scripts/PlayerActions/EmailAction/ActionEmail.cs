using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace INTENT
{
    public enum EmailType
    {
        Sending,
        Receiving
    }
    public class ActionEmail : PlayerAction
    {
        [SerializeField] private EmailType emailType;
        [SerializeField] private Button emailButton;
        [SerializeField] private GameObject inboxButtonPage;
        [SerializeField] private GameObject composeButtonPage;
        [SerializeField] private GameObject emailPage;
        [SerializeField] private GameObject sentImage;
        private void OnEnable() 
        {
            GameManager.Instance.PlayerEnterAction();
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
            switch(emailType)
            {
                case EmailType.Sending:
                    emailButton.onClick.AddListener(OpenCompose);
                    break;
                case EmailType.Receiving:
                    emailButton.onClick.AddListener(OpenInbox);
                    break;
            }
        }
        public void OpenInbox()
        {
            inboxButtonPage.SetActive(true);
        }
        public void OpenCompose()
        {
            composeButtonPage.SetActive(true);
        }
        public void SendEmail()
        {
            sentImage.SetActive(true);
            StartCoroutine(DelayBeforeSucces(1f));
        }
        public void ReadEmail()
        {
            inboxButtonPage.SetActive(false);
            emailPage.SetActive(true);
            StartCoroutine(DelayBeforeSucces(1f));
        }
        public override void PerformAction()
        {
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
            GameManager.Instance.PlayerExitAction();
            emailButton.onClick.RemoveAllListeners();
            SuccessFinishAction();
        }

        IEnumerator DelayBeforeSucces(float sec)
        {
            yield return new WaitForSeconds(sec);
            PerformAction();
        }
    }
}
