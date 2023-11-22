using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

namespace INTENT
{
    public class TakeawayPanelControl : Singleton<TakeawayPanelControl>, ISaveable
    {
        [SerializeField] SerializableDictionary<string, TakeawayCardControl> Cards;

        public void SetState(string card, string state)
        {
            if(!Cards.ContainsKey(card))
            {
                Debug.LogError("didn't find card "+card);
                return;
            }
            Cards[card].SetState(state);
        }

        public string GetIdentifier()
        {
            return "TakeawayPanel";
        }

        public Dictionary<string, string> GetSaveData()
        {
            var saveData = new Dictionary<string, string>();
            foreach (var card in Cards)
            {
                saveData.Add(card.Key, card.Value.CardState);
            }
            return saveData;
        }

        public void SetSaveData(Dictionary<string, string> saveData)
        {
            foreach (var card in Cards)
            {
                if(saveData.ContainsKey(card.Key))
                {
                    card.Value.SetState(saveData[card.Key]);
                }
            }
        }
    }
}
