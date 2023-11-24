using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace INTENT
{
    public class Cable : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerClickHandler, IPointerMoveHandler
    {
        private Camera mainCamera;

        public bool IsSelected = false;
        [SerializeField] private GameObject moveArea;
        [SerializeField] private LayerMask moveLayer;
        [SerializeField] private IndicatorSphereControl indicateSphere;
        private Vector3 startPos;
        // Start is called before the first frame update
        private void Awake()
        {
            startPos = this.transform.position;
            mainCamera = Camera.main;
            moveArea.SetActive(false);
        }
        private void OnEnable() 
        {            
            indicateSphere.gameObject.SetActive(true);
        }

        private void OnTriggerEnter(Collider other) 
        {
            if(other.CompareTag("ConnectPort"))
            {
                this.transform.position = other.transform.GetChild(0).position;
                this.transform.rotation = other.transform.GetChild(0).rotation;
                other.transform.parent.GetComponent<Projector>().Connected();
                moveArea.SetActive(false);
                this.enabled = false;
            }
        }
        

        public void OnDrag(PointerEventData eventData)
        {
            moveArea.SetActive(true);
            UpdatePosition(eventData);
            indicateSphere.gameObject.SetActive(false);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            moveArea.SetActive(false);
            IsSelected = false;
            indicateSphere.gameObject.SetActive(true);
        }

        public void ResetCable()
        {
            IsSelected = false;
            moveArea.SetActive(false);
            indicateSphere.gameObject.SetActive(false);
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if(!IsSelected)
            {
                indicateSphere.gameObject.SetActive(false);
                IsSelected = true;
                moveArea.SetActive(true);

                UpdatePosition(eventData);
            }
            else
            {
                IsSelected = false;
                moveArea.SetActive(false);
                indicateSphere.gameObject.SetActive(true);
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
