using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace INTENT
{
    public class CoffeeBeans : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler, IPointerMoveHandler
    {
        private Camera mainCamera;
        private Vector3 originPos;
        private Vector3 spawnPos;
        private GameObject beanModel;
        private Animator animator;
        private PointerEventData pointerEventData;
        public bool IsSelected = false;
        [SerializeField] private GameObject moveArea;
        [SerializeField] private LayerMask moveLayer;
        // Start is called before the first frame update
        void Awake()
        {
            mainCamera = Camera.main;
            beanModel = transform.GetChild(0).gameObject;
            animator = beanModel.GetComponent<Animator>();
            originPos = transform.position;
            Debug.Log("Bean pos: " + originPos);
            moveArea.SetActive(false);
        }
        public void ResetBean()
        {
            this.gameObject.SetActive(true);
            transform.position = originPos;
            animator.SetTrigger("Reset");
            IsSelected = false;
            moveArea.SetActive(false);
            Debug.Log("Reset Bean");
        }

        private void OnTriggerEnter(Collider other) 
        {
            if(other.CompareTag("CoffeeMachine"))
            {
                animator.SetTrigger("PourIn");
                OnEndDrag(pointerEventData);
                moveArea.SetActive(false);
                StartCoroutine(PouringIn(2f, other.GetComponent<CoffeeMachine>()));
                //this.gameObject.SetActive(false);
            }
        }
        IEnumerator PouringIn(float sec, CoffeeMachine coffeeMachine)
        {
            animator.SetTrigger("PourIn");
            pointerEventData.pointerDrag = null;
            yield return new WaitForSeconds(sec);
            this.gameObject.SetActive(false);
            coffeeMachine.BeanIn();
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            moveArea.SetActive(true);            
            pointerEventData = eventData;
            Debug.Log("Begin Dragging: " + pointerEventData.pointerDrag);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if(moveArea.activeSelf)
                UpdatePosition(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            moveArea.SetActive(false);
            transform.position = new Vector3(transform.position.x, originPos.y, transform.position.z);
            IsSelected = false;
        }

        
        public void OnPointerClick(PointerEventData eventData)
        {
            if(!IsSelected)
            {
                IsSelected = true;
                moveArea.SetActive(true);

                UpdatePosition(eventData);
            }
            else
            {
                IsSelected = false;
                transform.position = new Vector3(transform.position.x, originPos.y, transform.position.z);
                moveArea.SetActive(false);
            }
        }
        
        public void OnPointerMove(PointerEventData eventData)
        {
            if(IsSelected)
            {
                UpdatePosition(eventData);
            }
        }

        public void UpdatePosition(PointerEventData eventData)
        {
            Ray ray = mainCamera.ScreenPointToRay(eventData.position);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 100f, moveLayer))
            {
                transform.position = hit.point;
            }
        }
    }
}
