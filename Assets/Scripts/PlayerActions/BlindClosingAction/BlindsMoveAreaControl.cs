using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace INTENT
{
    public class BlindsMoveAreaControl : MonoBehaviour, IPointerMoveHandler, IPointerClickHandler
    {


        [SerializeField] private BlindHandle currentHandle;

        public void SetCurrentHandle(BlindHandle handle)
        {
            currentHandle = handle;
        }
        public void OnPointerMove(PointerEventData eventData)
        {
            if(currentHandle != null)
            {
                if (currentHandle.IsSelected)
                {
                    currentHandle.UpdatePosition(eventData);
                }
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(currentHandle != null)
            {
                this.gameObject.SetActive(false);
                currentHandle.ToggleSelected(false);
            }
        }
    }
}
