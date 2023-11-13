using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace INTENT
{
    public class BlindHandle : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerClickHandler, IPointerMoveHandler
    {
        private Camera mainCamera;
        [SerializeField] private ActionBlinds actionBlinds;
        [SerializeField] private GameObject blind;
        private float originY;

        public bool IsSelected = false;
        [SerializeField] private BlindsMoveAreaControl moveArea;
        [SerializeField] private LayerMask moveLayer;
        // Start is called before the first frame update
        void Start()
        {
            mainCamera = Camera.main;
            moveArea.gameObject.SetActive(false);
            originY = transform.position.y;
        }

        
        public void ToggleSelected(bool select)
        {
            IsSelected = select;
        }
        public void OnDrag(PointerEventData eventData)
        {
            moveArea.gameObject.SetActive(true);
            moveArea.SetCurrentHandle(this);
            UpdatePosition(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            moveArea.SetCurrentHandle(null);
            moveArea.gameObject.SetActive(false);
            IsSelected = false;
        }

        
        public void OnPointerClick(PointerEventData eventData)
        {
            if(!IsSelected)
            {
                IsSelected = true;
                moveArea.gameObject.SetActive(true);
                moveArea.SetCurrentHandle(this);

                UpdatePosition(eventData);
            }
            else
            {
                IsSelected = false;
                moveArea.SetCurrentHandle(null);
                moveArea.gameObject.SetActive(false);
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
                transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
                float newScaleY = (transform.position.y - originY) * (8f/-2.37f) + 1 ;
                blind.transform.localScale = new Vector3(blind.transform.localScale.x, newScaleY , blind.transform.localScale.z);
                if(newScaleY >= 9f)
                {
                    this.enabled = false;
                    moveArea.SetCurrentHandle(null);
                    moveArea.gameObject.SetActive(false);
                    actionBlinds.CloseBlind();
                }
            }
        }
    }
}
