using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace INTENT
{

    public class Doors : MonoBehaviour
    {
        #region Components
        [Header("GameObject Components")]
        [Tooltip("The teleport location for the player")]
        [SerializeField] private Transform teleportTrans;
        public Transform TeleportTrans { get { return teleportTrans; } }
        #endregion


        #region Door Settings
        [Header("Door Settings")]
        [Tooltip("The door this door is connected to")]
        [SerializeField] private Doors targetDoor;
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

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player" && targetDoor != null)
            {
                other.GetComponent<PlayerController>().TeleportPlayer(targetDoor.TeleportTrans.position, targetDoor.TeleportTrans.rotation);
            }

        }
    }

}