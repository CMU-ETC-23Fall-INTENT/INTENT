using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

namespace INTENT
{
    public class TakeawayPanelControl : Singleton<TakeawayPanelControl>, ISaveable
    {
        [SerializeField] private List<TakeawayCardControl> Cards;

        public void SetState(int index, string state)
        {
            if(Cards.Count <= index)
            {
                Debug.LogError("Index out of range");
                return;
            }
            Cards[index].SetState(state);
        }

        public string GetIdentifier()
        {
            return "TakeawayPanel";
        }

        public Dictionary<string, string> GetSaveData()
        {
            var saveData = new Dictionary<string, string>();
            foreach (TakeawayCardControl card in Cards)
            {
                var index = Cards.IndexOf(card);
                saveData.Add(index.ToString(), card.CardState);
            }
            return saveData;
        }

        public void SetSaveData(Dictionary<string, string> saveData)
        {
            foreach (TakeawayCardControl card in Cards)
            {
                var index = Cards.IndexOf(card);
                string cardState;
                if (saveData.TryGetValue(index.ToString(), out cardState))
                {
                    card.SetState(cardState);
                }
            }
        }
    }
}
