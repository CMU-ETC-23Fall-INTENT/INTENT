using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace INTENT
{
    public class SlotButton : MonoBehaviour, IPointerClickHandler
    {
        private MachineSlot slot;

        [Header("Outer Ring")]
        [SerializeField] private GameObject indicatorRing;
        [SerializeField] private Material idleMaterial;
        [SerializeField] private Material inStockMaterial;
        [SerializeField] private Material noStockMaterial;

        [Header("Inner Circle")]
        [SerializeField] private GameObject innerCircle;
        [SerializeField] private Material purchasedMaterial;
        [SerializeField] private Material notPurchasedMaterial;
        private bool canPurchase;
        private void Awake() 
        {
            slot = transform.parent.GetComponent<MachineSlot>();
        }
        public void ButtonLightUp(bool haveProduct)
        {
            switch (haveProduct)
            {
                case true:
                    canPurchase = true;
                    indicatorRing.GetComponent<MeshRenderer>().material = inStockMaterial;
                    break;
                case false:
                    canPurchase = false;
                    indicatorRing.GetComponent<MeshRenderer>().material = noStockMaterial;
                    break;
            }
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            Material currentMaterial = indicatorRing.GetComponent<MeshRenderer>().material;
            if(!canPurchase)
                return;
            canPurchase = false;
            innerCircle.GetComponent<MeshRenderer>().material = purchasedMaterial;
            slot.ProductPurchased();
            ResetButton(2f);
        }
        public void ResetButton(float sec)
        {
            canPurchase = false;
            StartCoroutine(ResetButtonCoroutine(sec));
        }
        IEnumerator ResetButtonCoroutine(float sec)
        {
            Debug.Log("Reset Button " + gameObject.name);
            yield return new WaitForSeconds(sec);
            innerCircle.GetComponent<MeshRenderer>().material = notPurchasedMaterial;
            indicatorRing.GetComponent<MeshRenderer>().material = idleMaterial;
            canPurchase = false;
        }
    }
}
