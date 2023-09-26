using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Doors : MonoBehaviour
{
    [SerializeField] private Doors targetDoor;
    // Start is called before the first frame update
    void OnDrawGizmosSelected()
    {
        
        if(targetDoor != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, targetDoor.transform.position);
            Debug.Log("Target door is not null");
        }
    }
}
