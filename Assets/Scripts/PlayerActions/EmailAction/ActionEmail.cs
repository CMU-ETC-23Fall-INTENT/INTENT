using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

namespace INTENT
{
    public enum EmailType
    {
        ToTony,
        FromManager,
        ToTonyFromTony
    }
    public class ActionEmail : PlayerAction
    {
        [SerializeField] private EmailType emailType;
        [SerializeField] private Button emailButton;
        [SerializeField] private Button inboxButton;
        [SerializeField] private Button composeButton;
        [SerializeField] private GameObject emailPage;
        [SerializeField] private GameObject fromManagerPage;
        [SerializeField] private GameObject fromTonyPage;
        [SerializeField] private GameObject sendToTonyPage;
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
                case EmailType.ToTony:
                    EnableButton(inboxButton, false, false);
                    EnableButton(composeButton, true, false);
                    break;
                case EmailType.FromManager:
                    EnableButton(inboxButton, true, true);
                    EnableButton(composeButton, false, false);
                    break;
                case EmailType.ToTonyFromTony:
                    EnableButton(inboxButton, false, false);
                    EnableButton(composeButton, true, false);
                    break;
            }
        }
        [YarnCommand("ChangeEmailType")]
        public void ChangeEmailType(int type)
        {
            switch(type)
            {
                case 0:
                    emailType = EmailType.ToTony;
                    break;
                case 1:
                    emailType = EmailType.FromManager;
                    break;
                case 2:
                    emailType = EmailType.ToTonyFromTony;
                    break;
            }
        }
        public void OpenEmail()
        {
            switch(emailType)
            {
                case EmailType.ToTony:
                    EnableButton(inboxButton, false, false);
                    EnableButton(composeButton, true, false);
                    break;
                case EmailType.FromManager:
                    EnableButton(inboxButton, true, true);
                    EnableButton(composeButton, false, false);
                    break;
                case EmailType.ToTonyFromTony:
                    EnableButton(inboxButton, false, false);
                    EnableButton(composeButton, true, false);
                    break;
            }
            emailPage.SetActive(true);
        }
        private void EnableButton(Button button, bool enable, bool yellowDot)
        {
            button.interactable = enable;
            if(yellowDot)
            {
                button.transform.GetChild(2).gameObject.SetActive(true);
            }
            else
            {
                button.transform.GetChild(2).gameObject.SetActive(false);
            }
        }
        public void OpenInbox()
        {
            switch(emailType)
            {
                case EmailType.ToTony:
                    break;
                case EmailType.FromManager:
                    fromManagerPage.SetActive(true);
                    break;
                case EmailType.ToTonyFromTony:
                    fromTonyPage.SetActive(true);
                    break;
            }
        }
        public void OpenCompose()
        {
            switch(emailType)
            {
                case EmailType.ToTony:
                    sendToTonyPage.SetActive(true);
                    break;
                case EmailType.FromManager:
                    break;
                case EmailType.ToTonyFromTony:
                    sendToTonyPage.SetActive(true);
                    break;
            }
        }
        public void GotItButton()
        {
            switch(emailType)
            {
                case EmailType.ToTony:
                    break;
                case EmailType.FromManager:
                    fromManagerPage.SetActive(false);
                    break;
                case EmailType.ToTonyFromTony:
                    fromTonyPage.SetActive(false);
                    break;
            }
            StartCoroutine(DelayBeforeSuccess(0f));
        }
        public void SendEmail()
        {
            sentImage.SetActive(true);
            switch(emailType)
            {
                case EmailType.ToTony:
                    StartCoroutine(DelayBeforeSuccess(1f));
                    break;
                case EmailType.FromManager:
                    break;
                case EmailType.ToTonyFromTony:
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
            SuccessFinishAction();
            this.enabled = false;
        }
        IEnumerator DelayBeforeTonyEmail(float sec)
        {
            yield return new WaitForSeconds(sec);
            sendToTonyPage.SetActive(false);
            EnableButton(inboxButton, true, true);
            EnableButton(composeButton, false, false);
        }

        IEnumerator DelayBeforeSuccess(float sec)
        {
            yield return new WaitForSeconds(sec);
            PerformAction();
        }
    }
}
