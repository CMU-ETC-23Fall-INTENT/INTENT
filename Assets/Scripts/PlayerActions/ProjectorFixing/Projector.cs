using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace INTENT
{
    public class Projector : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private ActionProjectorFixing actionProjectorFixing;
        [SerializeField] private FloatText floatTextPrefab;
        [SerializeField] private Cable cable;
        private Animator animator;
        private bool connected;
        private bool firstFinished;
        private bool finished;
        private int clickCount;
        private void OnEnable() 
        {
            animator = GetComponent<Animator>();
            if(clickCount > 2)
            {
                cable.enabled = true;
            }
        }
        public void Connected()
        {
            connected = true;
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Clicked");
            if(connected && !finished)
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(eventData.position);
                FloatText floatText = Instantiate(floatTextPrefab, pos, Quaternion.identity);
                floatText.StartFloat("Starting...");
                StartCoroutine(DelayBeforePerformAction());
                animator.SetBool("Started", true);
                finished = true;
                
            }
            else if(!connected && !finished)
            {
                clickCount ++;
                Vector3 pos = Camera.main.ScreenToWorldPoint(eventData.position);
                FloatText floatText = Instantiate(floatTextPrefab, pos, Quaternion.identity);
                floatText.StartFloat("Not responding...");
                animator.SetTrigger("StartFlash");
                if(clickCount == 3 && !firstFinished)
                {
                    firstFinished = true;
                    StartCoroutine(DelayBeforePerformAction());
                }
            }
        }

        IEnumerator DelayBeforePerformAction()
        {
            yield return new WaitForSeconds(1f);
            actionProjectorFixing.PerformAction();
        }
    }
}
