using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using System;
using UnityEngine.EventSystems;
using Yarn.Unity;

namespace INTENT
{
    public class PlayerController : MonoBehaviour
    {
        #region Components

        //[Header("GameObject Components")]
        //CharacterController characterController;
        NavMeshAgent agent;
        [SerializeField] private LayerMask interactionPointLayer;

        #endregion



        #region Player Variables

        [Header("Player Settings")]

        [Tooltip("Pause time after teleporting")]
        [SerializeField] private float pauseTime;

        #endregion


        #region Input & Status Variables
        private Vector2 rawInputVector;
        private Vector3 faceVector;
        private float currentSpeed;
        private bool isTeleporting;
        public bool IsInAction;
        public bool IsInTutorial;
        public bool IsInInteraction;
        public UltimateInteractionPoint CurrInteractionPoint = null;
        #endregion

        private bool shouldPause => isTeleporting || IsInAction || IsInTutorial || IsInInteraction;

        #region Constant Directions
        private readonly Vector3 horizontalMovement = new Vector3(1, 0, -1).normalized;
        private readonly Vector3 verticalMovement = new Vector3(1, 0, 1).normalized;
        #endregion

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            if (agent == null)
            {
                Debug.LogError("NavMeshAgent not found");
            }
            agent.destination = transform.position;
        }

        private void FixedUpdate()
        {
            if (!shouldPause)
            {
                Vector3 deltaPosKeyboard = faceVector * currentSpeed * agent.speed * Time.fixedDeltaTime;

                // Keyboard > Mouse
                if (deltaPosKeyboard.magnitude > 0.01f)
                {
                    agent.destination = transform.position + deltaPosKeyboard; // stop the auto navigation
                    agent.velocity = faceVector * currentSpeed * agent.speed;
                }
                else if (Input.GetMouseButton(0))
                {
                    if(!EventSystem.current.IsPointerOverGameObject())
                    {
                        RaycastHit hit;

                        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, interactionPointLayer)) //click interaction point to start interaction
                        {
                                                        UltimateInteractionPoint hitPoint = hit.transform.gameObject.GetComponent<UltimateInteractionPoint>();
                            if (CurrInteractionPoint == hitPoint) //If in interaction point
                            {
                                if (hit.transform.gameObject == CurrInteractionPoint.gameObject)
                                OnInteraction();
                            }
                            else if(hitPoint.IndicatorSphere.activeSelf)
                            {
                                agent.destination = hit.transform.position;
                            }
                            else // not in interaction point
                            {
                                agent.destination = hit.point;
                            }
                        }
                        else if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, ~interactionPointLayer)) //avoid interaction point layer (sphere)
                        {
                            agent.destination = hit.point;
                        }
                    }
                }
            }
            else if(!IsInAction)
            {
                agent.velocity = Vector3.zero;
                agent.destination = transform.position; // stop the auto navigation
            }
        }

        //Gets called when interact input is pressed
        private void OnInteraction()
        {
            EventManager.Instance.PlayerEvents.InteractPressed();
            if (shouldPause)
                return;
        }

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

        }

        public void TeleportPlayer(Vector3 pos, Quaternion rot)
        {
            StartCoroutine(PauseMoveme(pauseTime)); // can be removed, see which is better
            agent.Warp(pos);
        }

        private IEnumerator PauseMoveme(float pauseSec)
        {
            isTeleporting = true;
            yield return new WaitForSeconds(pauseSec);
            isTeleporting = false;

        }

        private bool IsInFrontOfMe(Transform otherTransform)
        {
            Vector3 dir = otherTransform.position - transform.position;
            float angle = Vector3.Angle(transform.forward, dir);
            return angle < 45f;
        }



    }
}
