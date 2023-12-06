using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace INTENT
{
    
    public class BlindHandle : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerClickHandler, IPointerMoveHandler
    {
        private Camera mainCamera;
        [SerializeField] private GameObject indicatorArrow;
        [SerializeField] private GameObject indicatorSphere;
        [SerializeField] private ActionBlinds actionBlinds;
        [SerializeField] private GameObject blind;
        [SerializeField] private float fullCloseScaleY = 12f;
        [SerializeField] private float fullClosePosY = -3.5f;
        private float blindTargetScaleY;
        private float handleTargetPosY;
        private float originY;
        private float originBlindScaleY;

        public bool IsSelected = false;
        [SerializeField] private BlindsMoveAreaControl moveArea;
        [SerializeField] private LayerMask moveLayer;
        // Start is called before the first frame update
        void Awake()
        {
            mainCamera = Camera.main;
            moveArea.gameObject.SetActive(false);
            originY = transform.localPosition.y;
            originBlindScaleY = blind.transform.localScale.y;
            blindTargetScaleY = fullCloseScaleY;
            handleTargetPosY = fullClosePosY;
        }
        
        public void ToggleTargetClose(int close) 
        {
            switch(close)
            {
                case 2:
                    blindTargetScaleY = fullCloseScaleY;
                    handleTargetPosY = fullClosePosY;
                    break;
                case 1:
                    blindTargetScaleY = (fullCloseScaleY + 1) / 2f;
                    handleTargetPosY = (fullClosePosY + originY) / 2f;
                    break;
            }
        }
        public void EnableIndicators(bool enableArrow, bool enableSphere)
        {
            indicatorArrow.SetActive(enableArrow);
            indicatorSphere.SetActive(enableSphere);
        }
        public void FullCloseBlinds(bool close) 
        {
            switch(close)
            {
                case true:
                    blind.transform.localScale = new Vector3(blind.transform.localScale.x, fullCloseScaleY, blind.transform.localScale.z);
                    this.transform.localPosition = new Vector3(transform.localPosition.x, fullClosePosY, transform.localPosition.z);
                    break;
                case false:
                    blind.transform.localScale = new Vector3(blind.transform.localScale.x, originBlindScaleY, blind.transform.localScale.z);
                    this.transform.localPosition = new Vector3(transform.localPosition.x, originY, transform.localPosition.z);
                    break;
            }
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
            EnableIndicators(false, true);
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
                EnableIndicators(false, true);
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
            
            EnableIndicators(true, false);
            Ray ray = mainCamera.ScreenPointToRay(eventData.position);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 100f, moveLayer))
            {
                transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
                float newScaleY = (transform.localPosition.y - originY) * (fullCloseScaleY - 1f)/(fullClosePosY - originY) + 1 ;
                blind.transform.localScale = new Vector3(blind.transform.localScale.x, newScaleY , blind.transform.localScale.z);
                if(newScaleY >= blindTargetScaleY * 0.9f)
                {
                    SoundManager2D.Instance.PlaySFX("CurtainStop");
                    this.enabled = false;
                    EnableIndicators(false, false);
                    blind.transform.localScale = new Vector3(blind.transform.localScale.x, blindTargetScaleY , blind.transform.localScale.z);
                    transform.localPosition = new Vector3(transform.localPosition.x, handleTargetPosY, transform.localPosition.z);
                    moveArea.SetCurrentHandle(null);
                    moveArea.gameObject.SetActive(false);
                    actionBlinds.CloseBlind();
                }
            }
        }
    }
}
