using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INTENT
{
    public class ActionAgenda : PlayerAction
    {
        [Header("Desktop Page")]
        [SerializeField] private GameObject desktopPage;
        [SerializeField] private GameObject confirmReservePanel;

        [Header("Agenda Page")]
        [SerializeField] private GameObject agendaPage;
        [SerializeField] private GameObject greyAgendaComfirmButton;
        [SerializeField] private GameObject blueAgendaComfirmButton;

        [Header("Reservation Page")]
        [SerializeField] private GameObject reservationPage;
        [SerializeField] private GameObject greyReservationButton;
        [SerializeField] private GameObject blueReservationButton;
        [SerializeField] private GameObject reminderPanel;


        private GameObject currentPage;
        private int taskCount = 0;
        private int reservedRoom = 0;
        private void OnEnable() 
        {
            GameManager.Instance.PlayerEnterAction();
            currentPage = desktopPage;
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }
        public void OpenPage(GameObject page)
        {
            currentPage.SetActive(false);
            currentPage = page;
            currentPage.SetActive(true);
        }
        public void DragTaskCount(int count)
        {
            taskCount += count;
            Debug.Log("Task count " + taskCount);
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
        public void RoomButton(int roomNumber)
        {
            reservedRoom = roomNumber;
            greyReservationButton.SetActive(false);
            blueReservationButton.SetActive(true);
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
                OpenPage(desktopPage);
                confirmReservePanel.SetActive(true);
                StartCoroutine(DelayedPerform(2f));
            }
        }
        public override void PerformAction()
        {
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
            GameManager.Instance.PlayerExitAction();
            this.enabled = false;
            SuccessFinishAction();
        }
        IEnumerator DelayedPerform(float sec)
        {
            yield return new WaitForSeconds(sec);
            PerformAction();
        }
    }
}
