using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

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

        public Transform TeleportTrans { get { return teleportTrans; } }
        private DoorStatus doorStatus = DoorStatus.Unlocked;
        #endregion


        #region Door Settings
        [Header("Door Settings")]
        [Tooltip("The door this door is connected to")]
        [SerializeField] private Doors targetDoor;

        [Tooltip("The target room name")]
        [SerializeField] private string targetRoomName;
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
            }

        }

        public void ChangeLockStatus(DoorStatus status)
        {
            doorStatus = status;
            UpdateDoorLook();
        }

        private void UpdateDoorLook()
        {
        }
    }

}
