using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        [SerializeField] private Button inboxButton;
        [SerializeField] private GameObject inboxButtonPage;
        [SerializeField] private GameObject composeButtonPage;
        [SerializeField] private GameObject sendToTonyPage;
        [SerializeField] private GameObject tonyEmailPage;
        [SerializeField] private GameObject managerEmailPage;
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
                    emailButton.onClick.AddListener(OpenComposeButtonPage);
                    inboxButton.onClick.AddListener(delegate{OpenInbox(0);});
                    break;
                case EmailType.Receiving:
                    emailButton.onClick.AddListener(OpenInboxButtonPage);
                    break;
            }
        }
        public void OpenInboxButtonPage()
        {
            inboxButtonPage.SetActive(true);
        }
        public void OpenComposeButtonPage()
        {
            composeButtonPage.SetActive(true);
        }
        public void OpenInbox(int index)
        {
            inboxButtonPage.SetActive(false);
            switch(index)
            {
                case 0:
                    managerEmailPage.SetActive(true);
                    break;
                case 1:
                    tonyEmailPage.SetActive(true);
                    break;
            }
        }
        public void GotItButton(int index)
        {
            switch(index)
            {
                case 0:
                    managerEmailPage.SetActive(false);
                    break;
                case 1:
                    tonyEmailPage.SetActive(false);
                    break;
            }
            StartCoroutine(DelayBeforeSucces(0.1f));
            inboxButton.onClick.RemoveAllListeners();
        }
        public void SendEmail(int index)
        {
            sentImage.SetActive(true);
            switch(index)
            {
                case 0:
                    StartCoroutine(DelayBeforeSucces(1f));
                    break;
                case 1:
                    StartCoroutine(DelayBeforeTonyEmail(1f));
                    break;
            }
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
        IEnumerator DelayBeforeTonyEmail(float sec)
        {
            yield return new WaitForSeconds(sec);
            sendToTonyPage.SetActive(false);
            inboxButtonPage.SetActive(true);
            inboxButton.onClick.AddListener(delegate{OpenInbox(1);});
        }

        IEnumerator DelayBeforeSucces(float sec)
        {
            yield return new WaitForSeconds(sec);
            PerformAction();
        }
    }
}
