using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace INTENT
{
    public class UIManager : Singleton<UIManager>
    {
        [Header("Task Panel")]
        #region Task Panel
        [SerializeField] private GameObject taskPanel;
        [SerializeField] private GameObject toDoListPanel;
        [SerializeField] private GameObject doneListPanel;
        [SerializeField] private GameObject taskPrefab;
        [SerializeField] private Color doneColor;
        #endregion

        [Header("Task Button")]
        #region Task Button
        [SerializeField] private GameObject taskButton;
        [SerializeField] private Sprite normalSprite;
        [SerializeField] private Sprite clickedSprite;
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

        [Header("Task Popup")]
        #region Task Popup
        [SerializeField] private GameObject taskPopup;
        [SerializeField] private TextMeshProUGUI taskPopupTitle;
        [SerializeField] private TextMeshProUGUI taskPopupDescription;
        [SerializeField] private Color taskPopupNewColor;
        [SerializeField] private Color taskPopupDoneColor;
        #endregion


        public void OpenTaskPanel(bool open)
        {
            taskPanel.SetActive(open);
            taskButton.GetComponent<Image>().sprite = open ? clickedSprite : normalSprite;

        }
        public void OpenCharacterPanel(bool open)
        {
            characterPanel.SetActive(open);
            characterButton.GetComponent<Image>().sprite = open ? clickedCharacterSprite : normalCharacterSprite;
        }
        public void AddToDoTaskList(Task task)
        {
            GameObject taskObject = Instantiate(taskPrefab, toDoListPanel.transform);
            taskObject.name = task.TaskSO.TaskId;
            taskObject.transform.Find("TitleText").GetComponent<TextMeshProUGUI>().text = task.TaskSO.TaskTitle;
            taskObject.transform.Find("DescriptionText").GetComponent<TextMeshProUGUI>().text = task.TaskSO.TaksDescription;
            TaskPopupNotice(true, task);
        }
        public void AddDoneTaskList(Task task)
        {
            GameObject taskObject = toDoListPanel.transform.Find(task.TaskSO.TaskId).gameObject;
            taskObject.transform.SetParent(doneListPanel.transform);
            taskObject.transform.position = Vector3.zero;
            taskObject.transform.Find("Background").GetComponent<Image>().color = doneColor;
            TaskPopupNotice(false, task);
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
                    taskPopup.transform.Find("Background").GetComponent<Image>().color = taskPopupNewColor;
                    taskPopupTitle.GetComponent<TextMeshProUGUI>().text = "New Task!";
                    taskPopupDescription.GetComponent<TextMeshProUGUI>().text = task.TaskSO.TaskTitle;
                    break;
                case false:
                    taskPopup.transform.Find("Background").GetComponent<Image>().color = taskPopupDoneColor;
                    taskPopupTitle.GetComponent<TextMeshProUGUI>().text = "Task Done!";
                    taskPopupDescription.GetComponent<TextMeshProUGUI>().text = task.TaskSO.TaskTitle;
                    break;
            }
            yield return new WaitForSeconds(3f);
            taskPopup.SetActive(false);
        }
    }
}
