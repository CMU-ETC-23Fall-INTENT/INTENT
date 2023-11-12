using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace INTENT
{
    public class CableMoveAreaControl : MonoBehaviour, IPointerMoveHandler
    {

        [SerializeField] private Cable cable;
        // Start is called before the first frame update

        public void OnPointerMove(PointerEventData eventData)
        {
            if(cable)
            {
                if (cable.IsSelected)
                {
                    cable.UpdatePosition(eventData);
                }
            }
        }
    }
}
