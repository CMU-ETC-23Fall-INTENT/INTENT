using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INTENT
{
    public class TakeawayDetailPanelControl : MonoBehaviour
    {
        [SerializeField] private List<KeyLearningControl> keyLearnings;

        private int currentIndex = 0;
        public void Activate(int index)
        {
            currentIndex = index;

            for (int i = 0; i < keyLearnings.Count; i++)
            {
                if (i == index)
                {
                    keyLearnings[i].gameObject.SetActive(true);
                    keyLearnings[i].Activate(0);
                }
                else
                {
                    keyLearnings[i].gameObject.SetActive(false);
                }
            }

        }
    }
}
