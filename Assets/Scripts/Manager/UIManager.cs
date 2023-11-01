using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using Yarn.Unity;

namespace INTENT
{
    public class UIManager : Singleton<UIManager>
    {
        [Header("Task Panel")]
        #region Task Panel
        [SerializeField] private GameObject taskPanel;
        [SerializeField] private GameObject takeawayPanel;
        [SerializeField] private GameObject takeawayDetailPanel;
        [SerializeField] private GameObject toDoListPanel;
        [SerializeField] private GameObject doneListPanel;
        [SerializeField] private GameObject taskPrefab;
        [SerializeField] private Color doneColor;
        #endregion

        [Header("Task Button")]
        #region Task Button
        [SerializeField] private GameObject taskButton;
        [SerializeField] private GameObject taskButtonIndicator;
        [SerializeField] private Sprite normalSprite;
        [SerializeField] private Sprite clickedSprite;
        private bool isTaskButtonClicked = false;
        #endregion

        [Header("Character Panel")]
        #region Character Panel
        [SerializeField] private GameObject characterPanel;
        #endregion

        [Header("Character Button")]
        #region Character Button
        [SerializeField] private GameObject characterButton;
        [SerializeField] private Sprite normalCharacterSprite;
        [SerializeField] private Sprite clickedCharacterSprite;
        #endregion

        [Header("Learn Panel")]
        #region Learn Panel
        [SerializeField] private GameObject learnPanel;
        #endregion

        [Header("Learn Button")]
        #region Learn Button
        [SerializeField] private GameObject learnButton;
        [SerializeField] private Sprite normalLearnSprite;
        [SerializeField] private Sprite clickedLearnSprite;
        #endregion

        [Header("Task Popup")]
        #region Task Popup
        [SerializeField] private GameObject taskPopup;
        [SerializeField] private TextMeshProUGUI taskPopupTitle;
        [SerializeField] private TextMeshProUGUI taskPopupDescription;
        [SerializeField] private Sprite taskPopupNewBackground;
        [SerializeField] private Sprite taskPopupDoneBackground;
        #endregion


        [SerializeField] private CanvasGroup fade;

        public void OpenTaskPanel(bool open)
        {
            taskPanel.SetActive(open);
            taskButton.GetComponent<Image>().sprite = open ? clickedSprite : normalSprite;
            isTaskButtonClicked = true;
            ToggleIndication();
        }
        public void OpenCharacterPanel(bool open)
        {
            characterPanel.SetActive(open);
            characterButton.GetComponent<Image>().sprite = open ? clickedCharacterSprite : normalCharacterSprite;
        }

        public void OpenLearnPanel(bool open)
        {
            learnPanel.SetActive(open);
            learnButton.GetComponent<Image>().sprite = open ? clickedLearnSprite : normalLearnSprite;
        }
        public void AddToDoTaskList(Task task)
        {
            GameObject taskObject = Instantiate(taskPrefab, toDoListPanel.transform);
            taskObject.name = task.TaskSO.TaskId;
            taskObject.transform.Find("TitleText").GetComponent<TextMeshProUGUI>().text = task.TaskSO.TaskTitle;
            taskObject.transform.Find("DescriptionText").GetComponent<TextMeshProUGUI>().text = task.TaskSO.TaksDescription;
            TaskPopupNotice(true, task);
            isTaskButtonClicked = false;
            ToggleIndication();
        }
        public void AddDoneTaskList(Task task)
        {
            GameObject taskObject = toDoListPanel.transform.Find(task.TaskSO.TaskId).gameObject;
            taskObject.transform.SetParent(doneListPanel.transform);
            taskObject.transform.position = Vector3.zero;
            taskObject.transform.Find("Background").GetComponent<Image>().color = doneColor;
            TaskPopupNotice(false, task);
            ToggleIndication();
        }

        public void TaskPopupNotice(bool isNew, Task task)
        {
            StartCoroutine(TaskPopupNoticeCoroutine(isNew, task));
        }

        IEnumerator TaskPopupNoticeCoroutine(bool isNew, Task task)
        {
            taskPopup.SetActive(true);
            switch(isNew)
            {
                case true:
                    taskPopup.transform.Find("Background").GetComponent<Image>().sprite = taskPopupNewBackground;
                    taskPopupTitle.GetComponent<TextMeshProUGUI>().text = "New Task!";
                    taskPopupDescription.GetComponent<TextMeshProUGUI>().text = task.TaskSO.TaskTitle;
                    break;
                case false:
                    taskPopup.transform.Find("Background").GetComponent<Image>().sprite = taskPopupDoneBackground;
                    taskPopupTitle.GetComponent<TextMeshProUGUI>().text = "Task Done!";
                    taskPopupDescription.GetComponent<TextMeshProUGUI>().text = task.TaskSO.TaskTitle;
                    break;
            }
            yield return new WaitForSeconds(3f);
            taskPopup.SetActive(false);
        }

        private void ToggleIndication()
        {
            taskButtonIndicator?.SetActive(toDoListPanel.transform.childCount > 0 && !isTaskButtonClicked);
        }

        

        
        [YarnCommand("FadeOut")]
        public Coroutine FadeOut(float sec)
        {
            return StartCoroutine(StartFadeOut(sec));
        }
        IEnumerator StartFadeOut(float sec)
        {
            float timer = 0;
            fade.blocksRaycasts = true;
            while(timer < (sec))
            {
                fade.alpha = Mathf.Lerp(0, 1, timer / (sec));
                timer += Time.deltaTime;
                yield return null;
            }
            fade.alpha = 1;
        }


        [YarnCommand("FadeIn")]
        public Coroutine FadeIn(float sec)
        {
            return StartCoroutine(StartFadeIn(sec));
        }
        IEnumerator StartFadeIn(float sec)
        {
            float timer = 0;
            fade.blocksRaycasts = false;
            while(timer < (sec))
            {
                fade.alpha = Mathf.Lerp(1, 0, timer / (sec));
                timer += Time.deltaTime;
                yield return null;
            }
            fade.alpha = 0;
        }

        public void TaskPanelSwitchToTakeawayPanel()
        {
            taskPanel.SetActive(false);
            takeawayPanel.SetActive(true);
        }
        public void TakeawayPanelSwitchToTaskPanel()
        {
            taskPanel.SetActive(true);
            takeawayPanel.SetActive(false);
        }
        public void ClosePanel(GameObject panel)
        {
            panel.SetActive(false);
        }

        public void TakeawayPanelSwitchToDetailPanel(int idx)
        {
            takeawayPanel.SetActive(false);
            takeawayDetailPanel.SetActive(true);
            takeawayDetailPanel.GetComponent<TakeawayDetailPanelControl>().Activate(idx);
        }

        public void DetailPanelBackToTakeawayPanel()
        {
            takeawayPanel.SetActive(true);
            takeawayDetailPanel.SetActive(false);
        }
    }
}
