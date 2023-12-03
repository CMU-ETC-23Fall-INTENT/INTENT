using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace INTENT
{
    public class ActionAgenda : PlayerAction
    {
        [Header("Desktop Page")]
        [SerializeField] private GameObject desktopPage;
        [SerializeField] private Button agendaButton;
        [SerializeField] private Button emailButton;
        [SerializeField] private GameObject confirmReservePanel;

        [Header("Agenda Page")]
        [SerializeField] private GameObject agendaPage;
        [SerializeField] private TaskSlots[] taskSlots;
        [SerializeField] private DraggableTask[] draggableTasks;
        [SerializeField] private GameObject greyAgendaComfirmButton;
        [SerializeField] private GameObject blueAgendaComfirmButton;

        [Header("Reservation Page")]
        [SerializeField] private GameObject reservationPage;
        [SerializeField] private GameObject greyReservationButton;
        [SerializeField] private GameObject blueReservationButton;
        [SerializeField] private GameObject reminderPanel;

        [Header("Tony Email Page")]
        [SerializeField] private GameObject tonyEmailPage;
        [TextArea(3, 10)]
        [SerializeField] private string correctOrderText;
        [TextArea(3, 10)]
        [SerializeField] private string wrongOrderText;
        [SerializeField] private TextMeshProUGUI tonyEmailText;


        private GameObject currentPage;
        private int taskCount = 0;
        private int reservedRoom = 0;
        private bool correctOrder;
        private bool reserved;
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
        }
        public override void ResetAction(int state)
        {
            reserved = false;
            taskCount = 0;
            reservedRoom = 0;
            correctOrder = false;
            OpenPage(desktopPage);
            foreach(TaskSlots slot in taskSlots)
            {
                slot.ResetSlots();
            }
            foreach(DraggableTask task in draggableTasks)
            {
                task.ResetToOrigin();
            }
        }
        public void OpenPage(GameObject page)
        {
            currentPage.SetActive(false);
            currentPage = page;
            currentPage.SetActive(true);
        }
        public void DragTaskCount(int count, bool isCorrect = false)
        {
            taskCount += count;
            correctOrder = isCorrect;
            if(taskCount == 3)
            {
                greyAgendaComfirmButton.SetActive(false);
                blueAgendaComfirmButton.SetActive(true);    
            }
            else
            {
                greyAgendaComfirmButton.SetActive(true);
                blueAgendaComfirmButton.SetActive(false);    
            }
        }
        public void ConfirmAgenda()
        {
            taskCount = 0;
            foreach(TaskSlots slot in taskSlots)
            {
                slot.ResetSlots();
            }
            foreach(DraggableTask task in draggableTasks)
            {
                task.ResetToOrigin();
            }
            switch(reserved)
            {
                case true:
                    OpenPage(desktopPage);
                    break;
                case false:
                    OpenPage(reservationPage);
                    break;
            }
            switch(correctOrder)
            {
                case true:
                    tonyEmailText.text = correctOrderText;
                    break;
                case false:
                    tonyEmailText.text = wrongOrderText;
                    break;
            }
        }
        public void RoomButton(int roomNumber)
        {
            reservedRoom = roomNumber;
            if(roomNumber != 0)
            {
                greyReservationButton.SetActive(false);
                blueReservationButton.SetActive(true);
            }
            else
            {
                greyReservationButton.SetActive(true);
                blueReservationButton.SetActive(false);
            }
            
        }
        public void ReminderOKButton()
        {
            reminderPanel.SetActive(false);
            reservedRoom = 0;
            greyReservationButton.SetActive(true);
            blueReservationButton.SetActive(false);
        }
        public void ConfirmReservationButton()
        {
            if(reservedRoom == 1)
            {
                reminderPanel.SetActive(true);
            }
            else if(reservedRoom == 2)
            {
                reserved = true;
                OpenPage(desktopPage);
                agendaButton.interactable = false;
                emailButton.interactable = true;
                StartCoroutine(confirmReserve(1f));
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
        }
        public void GotItButton()
        {
            switch(correctOrder)
            {
                case true:
                    PerformAction();
                    break;
                case false:
                    OpenPage(agendaPage);
                    break;
            }
        }
        IEnumerator confirmReserve(float sec)
        {
            confirmReservePanel.SetActive(true);
            yield return new WaitForSeconds(sec);
            confirmReservePanel.SetActive(false);

        }
    }
}
