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
        [SerializeField] private GameObject cardRoot;
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
            public bool IsTakeawayButtonNewBadgeActive = false;
            public Dictionary<string, bool> GameObjectsActiveState = new Dictionary<string, bool>();
        }

        public ISaveData GetSaveData()
        {
            TakeawayPanelSaveData saveData = new TakeawayPanelSaveData();
            foreach (var card in Cards)
            {
                saveData.Cards.Add(card.Key, card.Value.CardState);
            }
            saveData.IsTakeawayButtonNewBadgeActive = takeawayButton.gameObject.GetComponent<ButtonNewBadgeControl>().IsNewBadgeActive();
            GetGameObjectsActiveState(cardRoot, cardRoot, saveData.GameObjectsActiveState);
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
            takeawayButton.gameObject.GetComponent<ButtonNewBadgeControl>().SetNewBadgeActive(saveDataCast.IsTakeawayButtonNewBadgeActive);
            SetGameObjectsActiveState(cardRoot, cardRoot, saveDataCast.GameObjectsActiveState);
        }

        public string GetGameObjectRelativePath(GameObject target, GameObject root)
        {
            string path = target.name;
            Transform current = target.transform;
            while (current.parent != root.transform)
            {
                path = current.parent.name + "/" + path;
                current = current.parent;
            }
            return path;
        }

        public void GetGameObjectsActiveState(GameObject current, GameObject root, Dictionary<string, bool> gameObjectsActiveState)
        {
            foreach (Transform child in current.transform)
            {
                gameObjectsActiveState.Add(GetGameObjectRelativePath(child.gameObject, root), child.gameObject.activeSelf);
                GetGameObjectsActiveState(child.gameObject,root, gameObjectsActiveState);
            }
        }

        public void SetGameObjectsActiveState(GameObject current, GameObject root, Dictionary<string, bool> gameObjectsActiveState)
        {
            foreach (Transform child in current.transform)
            {
                if (gameObjectsActiveState.ContainsKey(GetGameObjectRelativePath(child.gameObject, root)))
                {
                    child.gameObject.SetActive(gameObjectsActiveState[GetGameObjectRelativePath(child.gameObject, root)]);
                }
                SetGameObjectsActiveState(child.gameObject, root, gameObjectsActiveState);
            }
        }

    }
}
