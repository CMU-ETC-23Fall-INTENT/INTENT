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
        [SerializeField] private string oneTimeText;
        [SerializeField] private string secondTimeText;
        [SerializeField] private string thirdTimeText;
        [SerializeField] private string overTimeText;
        private int makeCount = 0;

        private void OnEnable() 
        {
            
        }
        public void ResetMachine()
        {
            beanIn = false;
            finished = false;
            coffeeBeansIn.SetActive(false);
            coffeeLiquid.SetActive(false);
        }
        public void BeanIn()
        {
            beanIn = true;
            coffeeBeansIn.SetActive(true);
            SoundManager2D.Instance.PlaySFX("CoffeeGrind");
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(beanIn && !finished)
            {
                makeCount++;
                Vector3 pos = Camera.main.ScreenToWorldPoint(eventData.position);
                FloatText floatText = Instantiate(floatTextPrefab, pos, Quaternion.identity);
                switch(makeCount)
                {
                    case 1:
                        floatText.StartFloat(oneTimeText);
                        break;
                    case 2:
                        floatText.StartFloat(secondTimeText);
                        break;
                    case 3:
                        floatText.StartFloat(thirdTimeText);
                        break;
                    default:
                        floatText.StartFloat(overTimeText);
                        break;
                }
                SoundManager2D.Instance.PlaySFX("CoffeeOut");
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
            yield return new WaitForSeconds(1.5f);
            actionCoffeeMaking.PerformAction();
        }

    }
}
