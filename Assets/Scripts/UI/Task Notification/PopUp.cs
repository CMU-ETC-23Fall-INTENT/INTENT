using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace INTENT
{
    public class PopUp : MonoBehaviour
    {
        [SerializeField] Sprite newTaskSprite;
        [SerializeField] Sprite doneTaskSprite;
        private Image taskBackground;
        private TextMeshProUGUI newOrDoneText;
        private TextMeshProUGUI taskTitle;
        private RectTransform rectTransform;

        private void Awake()
        {
            taskBackground = this.transform.GetChild(0).GetComponent<Image>();
            newOrDoneText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            taskTitle = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            rectTransform = GetComponent<RectTransform>();
        }
        public void Initialize(bool isNew, string taskTitle)
        {
            switch(isNew)
            {
                case true:
                    newOrDoneText.text = "New Task!";
                    this.taskTitle.text = taskTitle;
                    taskBackground.sprite = newTaskSprite;
                    break;
                case false:
                    newOrDoneText.text = "Task Done!";
                    this.taskTitle.text = taskTitle;
                    taskBackground.sprite = doneTaskSprite;
                    break;
            }
        }
        public void SpawnPopIn(float inTime, float stayTime)
        {
            StartCoroutine(PopIn(inTime));
            StartCoroutine(PopUpFadeOut(stayTime, 0.5f));
            
        }
        IEnumerator PopIn(float sec)
        {
            float timer = 0f;
            Vector3 startPos = rectTransform.anchoredPosition;
            Vector3 endPos = Vector3.zero;
            while (timer < sec)
            {
                rectTransform.anchoredPosition = Vector3.Lerp(startPos, endPos, timer/sec);
                timer += Time.deltaTime;
                yield return null;
            }
            rectTransform.anchoredPosition = endPos;
        }
        public void StartMoveDown(float sec, float space)
        {
            StartCoroutine(MoveDown(sec, space));
        }
        IEnumerator MoveDown(float sec, float space)
        {
            float timer = 0f;
            Vector3 startPos = rectTransform.anchoredPosition;
            Vector3 endPos = rectTransform.anchoredPosition + Vector2.down * (GetComponent<RectTransform>().rect.height + space);
            while (timer < sec)
            {
                rectTransform.anchoredPosition = Vector3.Lerp(startPos, endPos, timer / sec);
                timer += Time.deltaTime;
                yield return null;
            }
            rectTransform.anchoredPosition = endPos;
        }
        IEnumerator PopUpFadeOut(float stayTime, float fadeoutTime)
        {
            yield return new WaitForSeconds(stayTime);
            float timer = 0f;
            while (timer < fadeoutTime)
            {
                this.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1, 0, timer / fadeoutTime);
                timer += Time.deltaTime;
                yield return null;
            }
            this.GetComponent<CanvasGroup>().alpha = 0;
            this.transform.parent.GetComponent<TaskPopUpPanel>().RemovePopUp();
        }
    }
}
