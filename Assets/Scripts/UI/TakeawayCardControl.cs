using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INTENT
{
    public class TakeawayCardControl : MonoBehaviour
    {
        [SerializeField] private GameObject unlocked;
        [SerializeField] private GameObject locked;
        public bool IsUnlocked { get; private set; }
        public void SetUnlocked(bool unlocked)
        {
            this.unlocked.SetActive(unlocked);
            this.locked.SetActive(!unlocked);
        }
    }
}
