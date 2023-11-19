using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

namespace INTENT
{
    public class TakeawayPanelControl : Singleton<TakeawayPanelControl>
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
    }
}
