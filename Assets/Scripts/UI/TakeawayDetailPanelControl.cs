using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INTENT
{
    public class TakeawayDetailPanelControl : MonoBehaviour
    {
        [SerializeField] private SerializableDictionary<string, KeyLearningControl> keyLearnings;

        private string currentKeyLearning = "";
        public void Activate(string str)
        {
            currentKeyLearning = str;

            foreach (var keyLearning in keyLearnings)
            {
                if (keyLearning.Key == str)
                {
                    keyLearning.Value.gameObject.SetActive(true);
                    keyLearning.Value.Activate(0);
                }
                else
                {
                    keyLearning.Value.gameObject.SetActive(false);
                }
            }
        }
    }
}
