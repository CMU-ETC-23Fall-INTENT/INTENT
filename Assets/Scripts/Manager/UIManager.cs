using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using Yarn.Unity;
using UnityEngine.AI;

namespace INTENT
{
    public class UIManager : Singleton<UIManager>
    {
        [Header("Task Panel")]
        #region Task Panel
        [SerializeField] private GameObject taskPanel;
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
        [SerializeField] private TakeawayPanelControl learnPanel;
        #endregion

        [Header("Learn Button")]
        #region Learn Button
        [SerializeField] private GameObject learnButton;
        [SerializeField] private Sprite normalLearnSprite;
        [SerializeField] private Sprite clickedLearnSprite;
        #endregion

        [Header("Task Popup")]
        #region Task Popup
        [SerializeField] private TaskPopUpPanel taskPopUpPanel;
        [SerializeField] private GameObject taskPopup;
        [SerializeField] private TextMeshProUGUI taskPopupTitle;
        [SerializeField] private TextMeshProUGUI taskPopupDescription;
        [SerializeField] private Sprite taskPopupNewBackground;
        [SerializeField] private Sprite taskPopupDoneBackground;
        #endregion


        [SerializeField] private CanvasGroup fade;
        private bool transitToEP2 = false;

        [YarnCommand("SetTransitToEP2")]
        public void SetTransitToEP2(bool value)
        {
            transitToEP2 = value;
        }
        public void OpenTaskPanel(bool open)
        {
            LoggingManager.Log("UI", "TaskPanel" + (open ? "Opened" : "Closed"));
            taskPanel.SetActive(open);
            taskButton.GetComponent<Image>().sprite = open ? clickedSprite : normalSprite;
            isTaskButtonClicked = true;
            ToggleIndication();
        }
        public void OpenCharacterPanel(bool open)
        {
            LoggingManager.Log("UI", "CharacterPanel" + (open ? "Opened":"Closed"));
            characterPanel.SetActive(open);
            characterButton.GetComponent<Image>().sprite = open ? clickedCharacterSprite : normalCharacterSprite;
        }
        public void TransitEP2OnClose()
        {
            if(transitToEP2)
            {
                transitToEP2 = false;
                StartCoroutine(ElevatorTransitionController.EpisodeTransition("One Mon", "th Later...", 2f, Episode.Episode2));
            }
        }
        public void OpenLearnPanel(bool open)
        {
            LoggingManager.Log("UI", "LearnPanel" + (open ? "Opened" : "Closed"));
            learnPanel.gameObject.SetActive(open);
            learnButton.GetComponent<Image>().sprite = open ? clickedLearnSprite : normalLearnSprite;
        }
        public void ClearAllTaskList()
        {
            foreach(Transform child in toDoListPanel.transform)
            {
                Destroy(child.gameObject);
            }
            foreach(Transform child in doneListPanel.transform)
            {
                Destroy(child.gameObject);
            }
        }
        public void AddToDoTaskList(Task task)
        {
            if(toDoListPanel.transform.Find(task.TaskSO.TaskId) != null)
            {
                return;
            }
            GameObject taskObject = Instantiate(taskPrefab, toDoListPanel.transform);
            taskObject.name = task.TaskSO.TaskId;
            taskObject.transform.Find("TitleText").GetComponent<TextMeshProUGUI>().text = task.TaskSO.TaskTitle;
            taskObject.transform.Find("DescriptionText").GetComponent<TextMeshProUGUI>().text = task.TaskSO.TaksDescription;
            taskPopUpPanel.AddPopUp(true, task.TaskSO.TaskTitle);
            isTaskButtonClicked = false;
            ToggleIndication();
        }
        public void AddDoneTaskList(Task task)
        {
            GameObject taskObject;
            var startedTask = this.toDoListPanel.transform.Find(task.TaskSO.TaskId);
            var doneTask = this.doneListPanel.transform.Find(task.TaskSO.TaskId);
            if(startedTask == null && doneTask == null)
            {
                taskObject = Instantiate(taskPrefab, doneListPanel.transform);
            }
            else if(doneTask != null)
            {
                return;
            }
            else
            {
                taskObject = startedTask.gameObject;
                taskObject.transform.SetParent(doneListPanel.transform);
            }
            taskObject.name = task.TaskSO.TaskId;
            taskObject.transform.position = Vector3.zero;
            taskObject.transform.Find("TitleText").GetComponent<TextMeshProUGUI>().text = task.TaskSO.TaskTitle;
            taskObject.transform.Find("DescriptionText").GetComponent<TextMeshProUGUI>().text = task.TaskSO.TaksDescription;
            taskObject.transform.Find("Background").GetComponent<Image>().color = doneColor;
            taskPopUpPanel.AddPopUp(false, task.TaskSO.TaskTitle);
            ToggleIndication();
        }
        public IEnumerator DelayedAdd(int type, Task task)
        {
            switch(type)
            {
                case 0:
                    yield return new WaitForSeconds(0.5f);
                    AddToDoTaskList(task);
                    break;
                case 1:
                    yield return new WaitForSeconds(0.5f);
                    AddDoneTaskList(task);
                    break;
            }
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
        public Coroutine FullFade(float fadeOut, float fadeIn)
        {
            return StartCoroutine(StartFullFade(fadeOut, fadeIn));
        }
        IEnumerator StartFullFade(float fadeOut, float fadeIn)
        {
            yield return StartCoroutine(StartFadeOut(fadeOut));
            yield return StartCoroutine(StartFadeIn(fadeIn));
        }

        public void ClosePanel(GameObject panel)
        {
            panel.SetActive(false);
        }

        public void TakeawayPanelSwitchToDetailPanel(int idx)
        {
            LoggingManager.Log("UI", "TakeawayPanelSwitchToDetailPanel" + string.Format("({0})", idx));
            OpenLearnPanel(false);
            takeawayDetailPanel.SetActive(true);
            takeawayDetailPanel.GetComponent<TakeawayDetailPanelControl>().Activate(idx);
        }

        public void DetailPanelBackToTakeawayPanel()
        {
            LoggingManager.Log("UI", "DetailPanelBackToTakeawayPanel");
            OpenLearnPanel(true);
            takeawayDetailPanel.SetActive(false);
        }

        [YarnCommand("SetTakeawayCardState")]
        public static void SetTakeawayCardState(string card, string state = "Unlocked", bool forceOpen = false)
        {
            LoggingManager.Log("UI", "SetTakeawayCardState: " + card);
            Instance.learnPanel.gameObject.SetActive(true);
            Instance.learnPanel.SetState(card, state);
            Instance.learnPanel.gameObject.SetActive(forceOpen);
        }
    }
}
