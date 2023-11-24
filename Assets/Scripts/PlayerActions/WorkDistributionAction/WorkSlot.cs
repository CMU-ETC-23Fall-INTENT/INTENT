using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Yarn.Unity;

namespace INTENT
{
    public class WorkSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private ActionDistribution actionDistribution;
        [SerializeField] private string npcIntro;
        [SerializeField] private string workType;
        private Image holderImage;
        private bool hasWork;
        private void Start() 
        {
            holderImage = transform.Find("Image").GetComponent<Image>();
        }
        public void ResetSlot()
        {
            hasWork = false;
            holderImage.GetComponent<CanvasGroup>().alpha = 0f;
        }
        public void OnDrop(PointerEventData eventData)
        {
            if(hasWork)
                return;
            if(actionDistribution.GetCurrentWork() != null)
            {
                SnapWork(actionDistribution.GetCurrentWork());
            }
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            if(hasWork)
                return;
            if(actionDistribution.GetCurrentWork() != null)
            {
                DraggableWork draggableWork = actionDistribution.GetCurrentWork();
                draggableWork.ToggelHover(true);
                PreviewImage(true, draggableWork.GetComponent<Image>().sprite);
            }
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            if(actionDistribution.GetCurrentWork() != null)
            {
                DraggableWork draggableWork = actionDistribution.GetCurrentWork();
                draggableWork.ToggelHover(false);
                PreviewImage(false);
            }
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if(hasWork)
                return;
            if(actionDistribution.GetCurrentWork() != null)
            {
                SnapWork(actionDistribution.GetCurrentWork());
            }
        }
        public void SnapWork(DraggableWork draggableWork)
        {
            draggableWork.transform.SetParent(transform);
            draggableWork.transform.localPosition = Vector2.zero;
            draggableWork.GetComponent<CanvasGroup>().blocksRaycasts = true;
            actionDistribution.StartDragWork(null);
            hasWork = true;
            draggableWork.ToggleDraggedIn(true);
            if(draggableWork.GetWorkType() != workType)
            {
                ChangeDialogue(draggableWork.GetWorkType(), false);
            }
            else
            {
                actionDistribution.AddWorkCount();
                if(actionDistribution.GetWorkCount() == 4)
                {
                    GameManager.Instance.GetDialogueRunner().onDialogueComplete.AddListener(actionDistribution.FinsihActionCallback);
                }
                ChangeDialogue(draggableWork.GetWorkType(), true);
                draggableWork.enabled = false;
            }
        }
        private void ChangeDialogue(string work, bool good)
        {
            GameManager.Instance.GetDialogueRunner().VariableStorage?.SetValue("$SlotNPCIntro", npcIntro);
            GameManager.Instance.GetDialogueRunner().VariableStorage?.SetValue("$DraggedInWork", work);
            switch(good)
            {
                case true:
                    GameManager.Instance.GetDialogueRunner().StartDialogue("EP2_07_WorkDistributionGood");
                    break;
                case false:
                    GameManager.Instance.GetDialogueRunner().StartDialogue("EP2_07_WorkDistributionBad");
                    break;
            }
        }
        public void RemoveWork()
        {
            hasWork = false;
        }
        private void PreviewImage(bool show, Sprite sprite = null)
        {
            switch(show)
            {
                case true:
                    holderImage.GetComponent<CanvasGroup>().alpha = 0.5f;
                    break;
                case false:
                    holderImage.GetComponent<CanvasGroup>().alpha = 0f;
                    break;
            }
            holderImage.sprite = sprite;
        }
    }
}
