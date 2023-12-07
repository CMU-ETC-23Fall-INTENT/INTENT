using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INTENT
{
    public class MachineSlot : MonoBehaviour
    {
        private ActionVenderMachine actionVenderMachine;
        [SerializeField] private SlotButton slotButton;
        [SerializeField] private GameObject products;
        private List<GameObject> productObjects = new List<GameObject>();
        private List<Vector3> productPositions = new List<Vector3>();
        private float productGap;
        private int soldProductCount;
        private void Awake() 
        {
            actionVenderMachine = transform.parent.GetComponent<ActionVenderMachine>();
            foreach(Transform child in products.transform)
            {
                productObjects.Add(child.gameObject);
                productPositions.Add(child.localPosition);
            }
            productGap = productObjects[0].transform.localPosition.z - productObjects[1].transform.localPosition.z;
        }
        public void ResetSlot()
        {
            soldProductCount = 0;
            for(int i = 0; i < soldProductCount; i++)
            {
                productObjects[i].SetActive(true);
                productObjects[i].GetComponent<Rigidbody>().isKinematic = true;
                productObjects[i].GetComponent<Rigidbody>().useGravity = false;
                productObjects[i].transform.localPosition = productPositions[i];
                productObjects[i].transform.localRotation = Quaternion.identity;
            }
        }
        public void ActivateSlot()
        {
            slotButton.ButtonLightUp(soldProductCount < productObjects.Count);
            Debug.Log("Slot Activated");
        }
        public void DeactivateSlot()
        {
            slotButton.ResetButton(0f);
            Debug.Log("Slot Deactivated");
        }
        public void ProductPurchased()
        {
            for(int i = soldProductCount; i < productObjects.Count; i++)
            {
                StartCoroutine(MoveProduct(productObjects[i], 1f, i == soldProductCount));
            }
            soldProductCount++;
            actionVenderMachine.StopAllButtons(this, soldProductCount == productObjects.Count);
        }
        private IEnumerator MoveProduct(GameObject product, float sec, bool drop = false)
        {
            float timer = 0f;
            Vector3 startPosition = product.transform.localPosition;
            Vector3 targetPosition = product.transform.localPosition + new Vector3(0f, 0f, productGap);
            while (timer < sec)
            {
                timer += Time.deltaTime;
                product.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, timer / sec);
                yield return null;
            }
            product.transform.localPosition = targetPosition;
            if(drop)
            {
                product.GetComponent<Rigidbody>().isKinematic = false;
                product.GetComponent<Rigidbody>().useGravity = true;
            }
        }
    }
}
