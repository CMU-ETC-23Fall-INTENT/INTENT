using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace INTENT
{
    public class DraggableTask : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public int TaskID;
        [SerializeField] private ActionAgenda actionAgenda;
        [SerializeField] private GameObject backgroundParent;
        private bool isIn;
        [SerializeField] private Vector3 originLocalPosition;

        public void ResetToOrigin()
        {
            transform.SetParent(backgroundParent.transform);
            transform.localPosition = originLocalPosition;
            TaskDragedIn(false);
        }
        
        public void TaskDragedIn(bool draggedIn)
        {
            GetComponent<CanvasGroup>().alpha = draggedIn ? 0f : 1f;
            isIn = draggedIn;
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
            if(isIn)
                actionAgenda.DragTaskCount(-1);
            
            TaskDragedIn(false);
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            GetComponent<CanvasGroup>().blocksRaycasts = false;
            if(isIn)
            {
                transform.parent.GetComponent<TaskSlots>().RemoveTask();
            }
            
            transform.SetParent(backgroundParent.transform);
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }
}
