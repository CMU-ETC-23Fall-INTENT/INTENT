using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


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

    #endregion



    

    #region Input & Status Variables
    private Vector2 rawInputVector;
    private Vector3 faceVector;
    private float currentSpeed;
    private bool isRotating;
    private IEnumerator turnCoroutine;

    #endregion



    #region Constant Directions
    private readonly Vector3 horizontalMovement = new Vector3(1, 0, -1).normalized;
    private readonly Vector3 verticalMovement = new Vector3(1, 0, 1).normalized;
    #endregion


    private void FixedUpdate() 
    {
        characterController.Move(faceVector * currentSpeed * maxSpeed * Time.fixedDeltaTime);
    }

    //Gets called when movement input value is changed
    private void OnMove(InputValue value)
    {
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

    //For rotating the player
    private IEnumerator StartTurn(Vector3 targetDir, float sec)
    {

        //Get the angle between the current forward vector and the target forward vector
        float angle = Vector3.Angle(transform.forward, faceVector);
        //Get the time needed to rotate
        float rotateTime = angle / 180f * sec;
        
        float timer = 0;
        Quaternion startRot = transform.rotation;
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
}
