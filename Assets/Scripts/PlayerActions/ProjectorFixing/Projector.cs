using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Yarn.Unity;

namespace INTENT
{
    public class Projector : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private ActionProjectorFixing actionProjectorFixing;
        [SerializeField] private GameObject switchIndicateSphere;
        [SerializeField] private GameObject portIndicateSphere;
        [SerializeField] private FloatText floatTextPrefab;
        [SerializeField] private Cable cable;
        [SerializeField] private GameObject screenImage;
        private Animator animator;
        private bool connected;
        private bool firstFinished;
        public bool Finished;
        private int clickCount;
        private void Awake() 
        {            
            animator = GetComponent<Animator>();
        }
        private void OnEnable() 
        {
            if(clickCount > 0)
            {
                portIndicateSphere.SetActive(true);
                cable.enabled = true;
            }
            else
            {
                switchIndicateSphere.SetActive(true);
            }
        }
        public void Connected()
        {
            portIndicateSphere.SetActive(false);
            switchIndicateSphere.SetActive(true);
            connected = true;
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            SoundManager2D.Instance.PlaySFX("ProjectorSwitch");
            if(connected && !Finished)
            {
                ConnectCable(eventData.position);
                switchIndicateSphere.SetActive(false);
                StartCoroutine(DelayBeforePerformAction());
                
            }
            else if(!connected && !Finished)
            {
                clickCount ++;
                switchIndicateSphere.SetActive(false);
                SoundManager2D.Instance.PlaySFX("ProjectorBreak");
                Vector3 pos = Camera.main.ScreenToWorldPoint(eventData.position);
                FloatText floatText = Instantiate(floatTextPrefab, pos, Quaternion.identity);
                floatText.StartFloat("Not responding...");
                animator.SetTrigger("StartFlash");
                if(clickCount == 1 && !firstFinished)
                {
                    firstFinished = true;
                    StartCoroutine(DelayBeforePerformAction());
                }
            }
        }
        private void ConnectCable(Vector3 clickPos)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(clickPos);
            FloatText floatText = Instantiate(floatTextPrefab, pos, Quaternion.identity);
            floatText.StartFloat("Starting...");
            ToggleScreen(true);
            Finished = true;
        }
        public void ResetProjector(int state)
        {
            ToggleScreen(false);           
            portIndicateSphere.SetActive(false);
            switchIndicateSphere.SetActive(false);
            Finished = false;
            connected = false;
            switch(state)
            {
                case 0:
                    clickCount = 0;
                    firstFinished = false;
                    break;
                case 1:
                    clickCount = 1;
                    firstFinished = true;
                    break;
            }
        }
        [YarnCommand("ToggleScreen")]
        public void ToggleScreen(bool toggle)
        {
            animator.SetBool("Started", toggle);
            screenImage.SetActive(toggle);
        }
        IEnumerator DelayBeforePerformAction()
        {
            yield return new WaitForSeconds(1f);
            actionProjectorFixing.PerformAction();
        }
    }
}
