using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INTENT
{
    public class CharacterPanelPerNPCControl : MonoBehaviour
    {
        [SerializeField] SerializableDictionary<string, GameObject> states;
        public string CardState { get; private set; } = "Locked";
        public void Awake()
        {
            SetState(CardState);
        }

        public void SetState(string state)
        {
            foreach (var cardState in states)
            {
                if (cardState.Key == state)
                {
                    cardState.Value.SetActive(true);
                }
                else
                {
                    cardState.Value.SetActive(false);
                }
            }
            CardState = state;
        }
    }
}
