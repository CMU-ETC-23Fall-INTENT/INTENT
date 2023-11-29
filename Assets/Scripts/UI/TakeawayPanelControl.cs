using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

namespace INTENT
{
    public class TakeawayPanelControl : Singleton<TakeawayPanelControl>, ISaveable
    {
        [SerializeField] SerializableDictionary<string, TakeawayCardControl> Cards;
        [SerializeField] private Button takeawayButton;

        public void SetState(string card, string state)
        {
            if(!Cards.ContainsKey(card))
            {
                Debug.LogError("didn't find card "+card);
                return;
            }
            Cards[card].SetState(state);
            takeawayButton.gameObject.GetComponent<ButtonNewBadgeControl>().ShowNewBadge();
        }

        public string GetIdentifier()
        {
            return "TakeawayPanel";
        }

        public class TakeawayPanelSaveData : ISaveData
        {
            public Dictionary<string, string> Cards = new Dictionary<string, string>();
        }

        public ISaveData GetSaveData()
        {
            TakeawayPanelSaveData saveData = new TakeawayPanelSaveData();
            foreach (var card in Cards)
            {
                saveData.Cards.Add(card.Key, card.Value.CardState);
            }
            return saveData;
        }

        public void SetSaveData(ISaveData saveData)
        {
            TakeawayPanelSaveData saveDataCast = (TakeawayPanelSaveData)saveData;
            foreach (var card in Cards)
            {
                if(saveDataCast.Cards.ContainsKey(card.Key))
                {
                    card.Value.SetState(saveDataCast.Cards[card.Key]);
                }
            }
        }
    }
}
