using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

namespace INTENT
{
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
        private bool isTeleporting;
        public bool IsHavingConversation; //Maybe public is a bad idea.
        private IEnumerator turnCoroutine;
        #endregion

        private bool shouldPause => isTeleporting || IsHavingConversation;

        #region Constant Directions
        private readonly Vector3 horizontalMovement = new Vector3(1, 0, -1).normalized;
        private readonly Vector3 verticalMovement = new Vector3(1, 0, 1).normalized;
        #endregion



        private void FixedUpdate()
        {
            if (!shouldPause)
                characterController.Move(faceVector * currentSpeed * maxSpeed * Time.fixedDeltaTime);
        }

        //Gets called when interact input is pressed


        //Gets called when movement input value is changed
        private void OnMove(InputValue value)
        {
            if (shouldPause)
            {
                faceVector = Vector3.zero;
                return;
            }
            rawInputVector = value.Get<Vector2>();
            currentSpeed = rawInputVector.magnitude;
            faceVector = (horizontalMovement * rawInputVector.x + verticalMovement * rawInputVector.y).normalized;

            if (transform.forward != faceVector)
            {
                if (isRotating)
                {
                    StopCoroutine(turnCoroutine);
                }
                turnCoroutine = StartTurn(faceVector, rotateTime);
                StartCoroutine(turnCoroutine);
            }
            if (currentSpeed == 0 && isRotating)
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
            isTeleporting = true;
            yield return new WaitForSeconds(pauseSec);
            isTeleporting = false;

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
            if (targetDir == Vector3.zero)
                targetDir = transform.forward;
            Quaternion targetRot = Quaternion.LookRotation(targetDir, Vector3.up);
            isRotating = true;
            while (timer < rotateTime)
            {
                transform.rotation = Quaternion.Slerp(startRot, targetRot, timer / rotateTime);
                timer += Time.deltaTime;
                yield return null;
            }
            isRotating = false;
        }

        
        private void OnInteraction()
        {
            EventManager.Instance.PlayerEvents.InteractPressed();
            Debug.Log("OnInteraction");
            if (shouldPause)
                return;
        }

        private bool IsInFrontOfMe(Transform otherTransform)
        {
            Vector3 dir = otherTransform.position - transform.position;
            float angle = Vector3.Angle(transform.forward, dir);
            return angle < 45f;
        }

    }
}
