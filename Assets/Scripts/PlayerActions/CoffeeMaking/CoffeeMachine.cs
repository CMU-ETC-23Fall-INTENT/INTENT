using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace INTENT
{
    public class CoffeeMachine : MonoBehaviour, IPointerClickHandler
    {        
        private bool beanIn = false;
        private bool finished = false;
        [SerializeField] private GameObject coffeeBeansIn;
        [SerializeField] private GameObject coffeeLiquid;
        [SerializeField] private FloatText floatTextPrefab;
        [SerializeField] private ActionCoffeeMaking actionCoffeeMaking;
        private void OnEnable() 
        {
            
        }
        public void BeanIn()
        {
            beanIn = true;
            coffeeBeansIn.SetActive(true);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(beanIn && !finished)
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(eventData.position);
                FloatText floatText = Instantiate(floatTextPrefab, pos, Quaternion.identity);
                floatText.StartFloat("Nice ~");
                coffeeBeansIn.SetActive(false);
                coffeeLiquid.SetActive(true);
                StartCoroutine(DelayBeforePerformAction());
                finished = true;
            }
            else if(!beanIn && !finished)
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(eventData.position);
                FloatText floatText = Instantiate(floatTextPrefab, pos, Quaternion.identity);
                floatText.StartFloat("No beans...");

            }
        }
        IEnumerator DelayBeforePerformAction()
        {
            yield return new WaitForSeconds(0.5f);
            actionCoffeeMaking.PerformAction();
        }

    }
}
