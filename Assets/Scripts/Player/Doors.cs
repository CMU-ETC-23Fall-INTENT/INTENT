using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using Yarn.Unity;
using UnityEngine.ProBuilder.Shapes;

namespace INTENT
{

    public class Doors : MonoBehaviour
    {
        public enum DoorStatus
        {
            Unlocked,
            Locked
        }
        #region Components
        [Header("GameObject Components")]
        [Tooltip("The teleport location for the player")]
        [SerializeField] private Transform teleportTrans;
        
        [SerializeField] private TextMeshPro doorNameTag1;
        [SerializeField] private TextMeshPro doorNameTag2;
        [SerializeField] private GameObject doorLock;
        [SerializeField] private BoxCollider doorCollider;

        public Transform TeleportTrans { get { return teleportTrans; } }
        private DoorStatus doorStatus = DoorStatus.Unlocked;
        #endregion


        #region Door Settings
        [Header("Door Settings")]
        [Tooltip("The door this door is connected to")]
        [SerializeField] private Doors targetDoor;

        [Tooltip("The target room name")]
        [SerializeField] private string targetRoomName;
        [SerializeField] private string targetRoomBGM;
        #endregion


        #region Gizmos
        //Draws a line from the door to the target door
        void OnDrawGizmosSelected()
        {
            if (targetDoor != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(transform.position, targetDoor.transform.position);
            }
        }
        #endregion

        private void OnValidate() 
        {
            doorNameTag1.text = targetRoomName;
            doorNameTag2.text = targetRoomName;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player" && targetDoor != null)
            {
                other.GetComponent<PlayerController>().TeleportPlayer(targetDoor.TeleportTrans.position, targetDoor.TeleportTrans.rotation);
                if(targetRoomBGM != "")
                {
                    SoundManager2D.Instance.FadePlayBGM(targetRoomBGM);
                }
                else
                {
                    SoundManager2D.Instance.StopBGM();
                }
            }

        }

        [YarnCommand("ChangeLockStatus")]
        public void ChangeLockStatus(bool isOpen)
        {
            DoorStatus status;
            switch(isOpen)
            {
                case false:
                    doorStatus = DoorStatus.Locked;
                    status = DoorStatus.Locked;
                    doorCollider.enabled = false;
                    break;
                case true:
                    doorStatus = DoorStatus.Unlocked;
                    status = DoorStatus.Unlocked;
                    doorCollider.enabled = true;
                    break;
            }
            UpdateDoorLook(status);
        }

        private void UpdateDoorLook(DoorStatus status)
        {
            switch(status)
            {
                case DoorStatus.Unlocked:
                    doorLock.SetActive(false);
                    break;
                case DoorStatus.Locked:
                    doorLock.SetActive(true);
                    break;
            }
        }
    }

}
