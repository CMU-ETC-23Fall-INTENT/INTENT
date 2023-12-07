using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace INTENT
{
    public class CreditRoll : MonoBehaviour
    {
        [SerializeField] private GameObject vertcialLayout;
        [SerializeField] private float creditRollSpeed = 100f;
        [SerializeField] private CanvasGroup verticalLayoutCanvasGroup;
        [SerializeField] private CanvasGroup allCanvasGroup;

        [Header("Option Box")]
        [SerializeField] private GameObject optionBox;
        [SerializeField] private CanvasGroup optionElemntCanvasGroup;
        private RectTransform verticalLayoutTransform;
        private float creditHeight;
        private Vector2 optionBoxSize;

        private void Awake()
        {
            verticalLayoutTransform = vertcialLayout.GetComponent<RectTransform>();
            optionBoxSize = optionBox.GetComponent<RectTransform>().sizeDelta;
            ResetCreditRoll();
        }
        
        public void StartCreditRoll()
        {
            gameObject.SetActive(true);
            SoundManager2D.Instance.FadePlayBGM("CreditMusic");
            GameManager.Instance.PlayerEnterAction();
            StartCoroutine(FadeInCredit(1f));
        }
        private IEnumerator FadeInCredit(float sec)
        {
            float timer = 0f;
            while (timer < sec)
            {
                timer += Time.deltaTime;
                allCanvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / sec);
                yield return null;
            }
            StartCoroutine(StartRoll(creditRollSpeed));
            allCanvasGroup.alpha = 1f;
            timer = 0f;
            while (timer < sec)
            {
                timer += Time.deltaTime;
                verticalLayoutCanvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / sec);
                yield return null;
            }
            verticalLayoutCanvasGroup.alpha = 1f;
        }
        private IEnumerator StartRoll(float speed)
        {
            creditHeight = vertcialLayout.GetComponent<RectTransform>().rect.height;
            while (verticalLayoutTransform.anchoredPosition.y < creditHeight)
            {
                verticalLayoutTransform.anchoredPosition += Vector2.up * speed * Time.deltaTime;
                yield return null;
            }
            yield return new WaitForSeconds(3f);
            float timer = 0f;
            while (timer < 2f)
            {
                timer += Time.deltaTime;
                verticalLayoutCanvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / 2f);
                yield return null;
            }
            verticalLayoutCanvasGroup.alpha = 0f;
            verticalLayoutCanvasGroup.blocksRaycasts = false;
            verticalLayoutCanvasGroup.interactable = false;
            SoundManager2D.Instance.FadePlayBGM("OfficeSpace");
            StartCoroutine(OpenOptionBox());

        }
        private IEnumerator OpenOptionBox()
        {
            float timer = 0f;
            Vector2 originalSize = optionBox.GetComponent<RectTransform>().sizeDelta;
            while (timer < 0.5f)
            {
                timer += Time.deltaTime;
                optionBox.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(originalSize, optionBoxSize, timer / 0.5f);
                yield return null;
            }
            optionBox.GetComponent<RectTransform>().sizeDelta = optionBoxSize;
            timer = 0f;
            while (timer < 0.5f)
            {
                timer += Time.deltaTime;
                optionElemntCanvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / 0.5f);
                yield return null;
            }
            optionElemntCanvasGroup.alpha = 1f;
        }
        public void ExploreButton()
        {
            CloseCredit();
        }
        public void RestartButton()
        {
            CloseCredit();
            //Todo: Restart Game
        }
        private void CloseCredit()
        {
            StartCoroutine(CloseCreditCoroutine());
        }
        private void ResetCreditRoll()
        {
            verticalLayoutTransform.anchoredPosition = Vector2.zero;
            verticalLayoutCanvasGroup.alpha = 0f;
            verticalLayoutCanvasGroup.blocksRaycasts = true;
            verticalLayoutCanvasGroup.interactable = true;
            allCanvasGroup.alpha = 0f;
            optionBox.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, optionBoxSize.y);
            optionElemntCanvasGroup.alpha = 0f;
        }
        private IEnumerator CloseCreditCoroutine()
        {
            float timer = 0f;
            while (timer < 1f)
            {
                timer += Time.deltaTime;
                allCanvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / 1f);
                yield return null;
            }
            allCanvasGroup.alpha = 0f;
            gameObject.SetActive(false);
            ResetCreditRoll();
            GameManager.Instance.PlayerExitAction();
        }
    }
}
