using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

namespace INTENT{
    using DS;
public class PlayerController : MonoBehaviour
{
    #region Components

    [Header("GameObject Components")]
    [SerializeField] CharacterController characterController;

    #endregion



    #region Player Variables

    [Header("Player Settings")]

    [Tooltip("Player Speed")]
    [SerializeField] private float maxSpeed;

    [Tooltip("Time needed to rotate 180 degrees")]
    [SerializeField] private float rotateTime;

    [Tooltip("Pause time after teleporting")]
    [SerializeField] private float pauseTime;

    #endregion


    #region Input & Status Variables
    private Vector2 rawInputVector;
    private Vector3 faceVector;
    private float currentSpeed;
    private bool isRotating;
    private bool isPaused;
    private IEnumerator turnCoroutine;

    #endregion



    #region Constant Directions
    private readonly Vector3 horizontalMovement = new Vector3(1, 0, -1).normalized;
    private readonly Vector3 verticalMovement = new Vector3(1, 0, 1).normalized;
    #endregion


    
    private void FixedUpdate() 
    {
        if(!isPaused)
            characterController.Move(faceVector * currentSpeed * maxSpeed * Time.fixedDeltaTime);
    }

    //Gets called when movement input value is changed
    private void OnMove(InputValue value)
    {
        if(isPaused)
        {
            faceVector = Vector3.zero;
            return;
        }
        rawInputVector = value.Get<Vector2>();
        currentSpeed = rawInputVector.magnitude;
        faceVector = (horizontalMovement * rawInputVector.x + verticalMovement * rawInputVector.y).normalized;
        
        if(transform.forward != faceVector)
        {   
            if(isRotating)
            {
                StopCoroutine(turnCoroutine);
            }
            turnCoroutine = StartTurn(faceVector, rotateTime);                      
            StartCoroutine(turnCoroutine);
        }
        if(currentSpeed == 0 && isRotating)
        {
            StopCoroutine(turnCoroutine);  
            isRotating = false;
        }
    }

    public void TeleportPlayer(Vector3 pos, Quaternion rot)
    {
        characterController.enabled = false;
        StartCoroutine(PauseMoveme(pauseTime));
        
        transform.position = pos;
        transform.rotation = rot;

        characterController.enabled = true;
    }

    private IEnumerator PauseMoveme(float pauseSec)
    {
        isPaused = true;
        yield return new WaitForSeconds(pauseSec);
        isPaused = false;
        
    }



    //For rotating the player
    private IEnumerator StartTurn(Vector3 targetDir, float sec)
    {

        //Get the angle between the current forward vector and the target forward vector
        float angle = Vector3.Angle(transform.forward, faceVector);
        //Get the time needed to rotate
        float rotateTime = angle / 180f * sec;
        
        float timer = 0;
        Quaternion startRot = transform.rotation;
        if(targetDir == Vector3.zero)
            targetDir = transform.forward;
        Quaternion targetRot = Quaternion.LookRotation(targetDir, Vector3.up);
        isRotating = true;
        while(timer < rotateTime)
        {
            transform.rotation = Quaternion.Slerp(startRot, targetRot, timer / rotateTime);
            timer += Time.deltaTime;
            yield return null;
        }
        isRotating = false;
    }

    #region Trigger
    private List<Collider> triggerColliders = new List<Collider>();
    private bool IsInTrigger() => triggerColliders.Count > 0;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter: "+other.gameObject.name);
        triggerColliders.Add(other);
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("OnTriggerExit: "+other.gameObject.name);
        triggerColliders.Remove(other);
    }
    #endregion
    private void OnInteraction()
    {
        Debug.Log("OnInteraction");
        if(isPaused)
            return;
        
        if(IsInTrigger())
        {
            // Debug.Log("IsInTrigger");

            foreach (var collider in triggerColliders)
            {
                // Debug.Log("collider: "+collider.gameObject.name);
                if(collider.CompareTag("NPC") && IsInFrontOfMe(collider.transform))
                {
                    Debug.Log("Interacted with "+collider.gameObject.name+"!");
                    GameManager.Instance.StartDialogue(collider.GetComponent<DSDialogue>());
                }
            }
        }
    }

    private bool IsInFrontOfMe(Transform otherTransform)
    {
        Vector3 dir = otherTransform.position - transform.position;
        float angle = Vector3.Angle(transform.forward, dir);
        return angle < 45f;
    }

}
}