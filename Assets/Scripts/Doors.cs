using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Doors : MonoBehaviour
{
    [SerializeField] private Doors targetDoor;
    [field: SerializeField] public Transform TeleportTrans { get; private set; }
    // Start is called before the first frame update
    void OnDrawGizmosSelected()
    {        
        if(targetDoor != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, targetDoor.transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && targetDoor != null)
        {
            other.GetComponent<PlayerController>().TeleportPlayer(targetDoor.TeleportTrans.position, targetDoor.TeleportTrans.rotation);
        }
        
    }
}
