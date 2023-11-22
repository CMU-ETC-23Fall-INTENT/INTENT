using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace INTENT
{
    public class TaskSlots : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private int taskID;
        [SerializeField] private Sprite task1Image;
        [SerializeField] private Sprite task2Image;
        [SerializeField] private Sprite task3Image;

        [SerializeField] private ActionAgenda actionAgenda;
        [SerializeField] private DraggableTask currentTask;

        public void ResetSlots()
        {
            RemoveTask();
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            if(eventData.pointerDrag != null && currentTask == null)
            {
                DraggableTask draggableTask = eventData.pointerDrag.GetComponent<DraggableTask>();
                if(draggableTask != null)
                {
                    GetComponent<CanvasGroup>().alpha = 0.5f;
                    switch(draggableTask.TaskID)
                    {
                        case 1:
                            GetComponent<Image>().sprite = task1Image;
                            break;
                        case 2:
                            GetComponent<Image>().sprite = task2Image;
                            break;
                        case 3:
                            GetComponent<Image>().sprite = task3Image;
                            break;
                    }
                }
            }
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            if(eventData.pointerDrag != null)
            {
                DraggableTask draggableTask = eventData.pointerDrag.GetComponent<DraggableTask>();
                if(draggableTask != null)
                {
                    
                    if(currentTask == null)
                    {
                        GetComponent<CanvasGroup>().alpha = 0f;
                    }           
                }
            }
        }
        public void RemoveTask()
        {
            currentTask = null;
            GetComponent<CanvasGroup>().alpha = 0f;
        }
        public void OnDrop(PointerEventData eventData)
        {
            if(currentTask != null)
                return;
            if(eventData.pointerDrag != null)
            {
                DraggableTask draggableTask = eventData.pointerDrag.GetComponent<DraggableTask>();
                if(draggableTask != null)
                {                    
                    GetComponent<CanvasGroup>().alpha = 1;
                    draggableTask.TaskDragedIn(true);
                    draggableTask.transform.SetParent(transform);
                    draggableTask.transform.localPosition = Vector3.zero;
                    currentTask = draggableTask;
                    actionAgenda.DragTaskCount(1, draggableTask.TaskID == taskID);
                    switch(draggableTask.TaskID)
                    {
                        case 1:
                            GetComponent<Image>().sprite = task1Image;
                            break;
                        case 2:
                            GetComponent<Image>().sprite = task2Image;
                            break;
                        case 3:
                            GetComponent<Image>().sprite = task3Image;
                            break;
                    }
                }
            }
        }
    }
}
