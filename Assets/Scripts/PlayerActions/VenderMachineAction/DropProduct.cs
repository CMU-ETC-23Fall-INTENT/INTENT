using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace INTENT
{
    public class DropProduct : MonoBehaviour, IPointerClickHandler
    {
        private ActionVenderMachine actionVenderMachine;
        private void Awake() 
        {
            actionVenderMachine = transform.parent.parent.parent.GetComponent<ActionVenderMachine>();
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            PickUpProduct();
        }
        public void PickUpProduct()
        {
            actionVenderMachine.ProductPickedUp();
            this.gameObject.SetActive(false);
            Debug.Log("Pick Up Product");
        }
    }
}
