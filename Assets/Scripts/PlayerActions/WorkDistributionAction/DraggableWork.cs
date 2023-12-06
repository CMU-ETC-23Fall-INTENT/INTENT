using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace INTENT
{
    public class DraggableWork : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
    {
        [SerializeField] private ActionDistribution actionDistribution;
        [SerializeField] private string workType;
        private GameObject backgroundParent;
        private Camera mainCamera;
        private bool isIn;
        private bool isOnhand;
        private bool isHovering;
        private Vector3 startPos;
        private void Start()
        {
            startPos = transform.position;
            backgroundParent = transform.parent.gameObject;
            mainCamera = Camera.main;
        }
        public void ResetWork()
        {
            isIn = false;
            isOnhand = false;
            isHovering = false;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            transform.SetParent(backgroundParent.transform);
            transform.position = startPos;
        }
        public string GetWorkType()
        {
            return workType;
        }
        public void ToggleDraggedIn(bool draggedIn)
        {
            isIn = draggedIn;
        }
        public void ToggelHover(bool hover)
        {
            isHovering = hover;
        }
        public void OnDrag(PointerEventData eventData)
        {
            UpdatePosition(eventData);
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            SoundManager2D.Instance.PlaySFX("StickyTake");
            GetComponent<CanvasGroup>().blocksRaycasts = false;
            if(isIn)
            {
                transform.parent.GetComponent<WorkSlot>().RemoveWork();
                ToggleDraggedIn(false);
            }

        }
        public void OnEndDrag(PointerEventData eventData)
        {
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            actionDistribution.StartDragWork(null);
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if(!isOnhand)
            {                
                if(isIn)
                {
                    transform.parent.GetComponent<WorkSlot>().RemoveWork();
                    ToggleDraggedIn(false);
                }
                isOnhand = true;
                GetComponent<CanvasGroup>().blocksRaycasts = false;
                StartCoroutine(ClickDragging(eventData));
            }
        }

        private void UpdatePosition(PointerEventData eventData)
        {
            actionDistribution.StartDragWork(this);
            this.transform.position = mainCamera.ScreenToWorldPoint(eventData.position);
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, 0);
            this.transform.SetParent(backgroundParent.transform);
            this.transform.SetAsLastSibling();

        }
        private IEnumerator ClickDragging(PointerEventData eventData)
        {
            while(isOnhand)
            {
                UpdatePosition(eventData);
                if (Input.GetMouseButtonDown(0))
                {
                    if(!isHovering)
                    {
                        GetComponent<CanvasGroup>().blocksRaycasts = true;
                        actionDistribution.StartDragWork(null);
                    }
                    isOnhand = false;
                }
                yield return null;
            }
        }
    }
}
