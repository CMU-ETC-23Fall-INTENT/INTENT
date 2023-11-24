using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace INTENT
{
    public class CoffeeBeans : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerClickHandler, IPointerMoveHandler
    {
        private Camera mainCamera;
        private Vector3 originPos;

        public bool IsSelected = false;
        [SerializeField] private GameObject moveArea;
        [SerializeField] private LayerMask moveLayer;
        // Start is called before the first frame update
        void Awake()
        {
            mainCamera = Camera.main;
            originPos = transform.position;
            Debug.Log("Bean pos: " + originPos);
            moveArea.SetActive(false);
        }
        public void ResetBean()
        {
            IsSelected = false;
            moveArea.SetActive(false);
            transform.position = originPos;
            this.gameObject.SetActive(true);
        }

        private void OnTriggerEnter(Collider other) 
        {
            if(other.CompareTag("CoffeeMachine"))
            {
                other.GetComponent<CoffeeMachine>().BeanIn();
                moveArea.SetActive(false);
                this.gameObject.SetActive(false);
            }
        }
        

        public void OnDrag(PointerEventData eventData)
        {
            moveArea.SetActive(true);
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
