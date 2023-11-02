using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace INTENT
{
    public class CoffeeBeans : MonoBehaviour, IDragHandler
    {
        private Vector3 mousePosition;
        private Camera mainCamera;
        [SerializeField] private LayerMask moveLayer;
        // Start is called before the first frame update
        void Start()
        {
            mainCamera = Camera.main;
        }

        private void OnTriggerEnter(Collider other) 
        {
            if(other.CompareTag("CoffeeMachine"))
            {
                other.GetComponent<CoffeeMachine>().BeanIn();
                Destroy(this.gameObject);  
            }
        }
        

        public void OnDrag(PointerEventData eventData)
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
