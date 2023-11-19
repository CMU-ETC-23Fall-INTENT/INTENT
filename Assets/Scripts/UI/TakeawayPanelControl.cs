using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

namespace INTENT
{
    public class TakeawayPanelControl : Singleton<TakeawayPanelControl>, ISaveable
    {
        [SerializeField] private List<TakeawayCardControl> Cards;

        public void SetUnlocked(int index, bool unlocked)
        {
            if(Cards.Count <= index)
            {
                Debug.LogError("Index out of range");
                return;
            }
            Cards[index].SetUnlocked(unlocked);
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
                saveData.Add(index.ToString(), card.IsUnlocked.ToString());
            }
            return saveData;
        }

        public void SetSaveData(Dictionary<string, string> saveData)
        {
            foreach (TakeawayCardControl card in Cards)
            {
                var index = Cards.IndexOf(card);
                string unlocked;
                if (saveData.TryGetValue(index.ToString(), out unlocked))
                {
                    card.SetUnlocked(bool.Parse(unlocked));
                }
            }
        }
    }
}
