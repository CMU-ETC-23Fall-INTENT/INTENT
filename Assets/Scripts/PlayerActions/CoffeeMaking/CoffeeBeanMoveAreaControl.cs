using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace INTENT
{
    public class CoffeeBeanMoveAreaControl : MonoBehaviour, IPointerMoveHandler
    {

        [SerializeField] private CoffeeBeans coffeeBean;
        // Start is called before the first frame update

        public void OnPointerMove(PointerEventData eventData)
        {
            if(coffeeBean)
            {
                if (coffeeBean.IsSelected)
                {
                    coffeeBean.UpdatePosition(eventData);
                }
            }
        }
    }
}
