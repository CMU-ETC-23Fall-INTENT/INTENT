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
        }
        public void AddDoneTaskList(Task task)
        {
            GameObject taskObject = toDoListPanel.transform.Find(task.TaskSO.TaskId).gameObject;
            taskObject.transform.SetParent(doneListPanel.transform);
            taskObject.transform.position = Vector3.zero;
            taskObject.transform.Find("Background").GetComponent<Image>().color = doneColor;
        }
    }
}
