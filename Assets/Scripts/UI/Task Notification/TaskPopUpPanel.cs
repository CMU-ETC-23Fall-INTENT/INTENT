using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INTENT
{
    public class TaskPopUpPanel : MonoBehaviour
    {
        [SerializeField] private PopUp taskPopUpPrefab;
        [SerializeField] private float offScreenOffset;
        [SerializeField] private float popInTime;
        [SerializeField] private float popUpStayTime;
        [SerializeField] private float popUpMoveDownTime;
        [SerializeField] private float popUpSpace;
        private Queue<PopUp> poping = new Queue<PopUp>();
        private Queue<PopUp> popUps = new Queue<PopUp>();


        private void Start()
        {
            StartCoroutine(AddingPopUp());
        }
        public void AddPopUp(bool isNew, string taskTitle)
        {
            PopUp popUp = Instantiate(taskPopUpPrefab, Vector3.zero, Quaternion.identity, this.transform);
            poping.Enqueue(popUp);
            popUp.GetComponent<RectTransform>().anchoredPosition = new Vector3(taskPopUpPrefab.GetComponent<RectTransform>().rect.width + offScreenOffset, 0, 0);
            popUp.Initialize(isNew, taskTitle);
        }
        IEnumerator AddingPopUp()
        {
            while(true)
            {
                while (poping.Count > 0)
                {
                    poping.Peek().SpawnPopIn(popInTime, popUpStayTime);
                    foreach(PopUp popUp in popUps)
                    {
                        popUp.StartMoveDown(popUpMoveDownTime, popUpSpace);
                    }
                    yield return new WaitForSeconds(popInTime);
                    popUps.Enqueue(poping.Dequeue());
                }
                yield return null;
            }            
            
        }
       
        public void RemovePopUp()
        {
            Destroy(popUps.Dequeue().gameObject);
        }
        
    }
}
