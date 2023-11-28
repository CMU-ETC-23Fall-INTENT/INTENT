using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

namespace INTENT
{
    public class CharacterPanelSaveData : ISaveData
    {
        public Dictionary<string, string> States;
    }

    public class CharacterPanelControl : Singleton<CharacterPanelControl>, ISaveable
    {
        [SerializeField] SerializableDictionary<string, CharacterPanelPerNPCControl> NPCs;
        [SerializeField] private List<GameObject> characterPoints;
        [SerializeField] private NavBarControl characterNavBar;
        private int currentIndex = 0;

        public void Initialize()
        {
            characterNavBar.gameObject.SetActive(true);
            characterNavBar.Initialize(characterPoints.Count, Next, Prev, Activate);
            characterNavBar.gameObject.SetActive(false);
        }

        public void Awake()
        {
            Initialize();
        }

        public void OnEnable()
        {
            Activate(0);
        }

        public void Activate(int index)
        {
            currentIndex = index;

            characterNavBar.gameObject.SetActive(true);
            characterNavBar.UpdateNavBar(index);

            for (int i = 0; i < characterPoints.Count; i++)
            {
                characterPoints[i].SetActive(i == index);
            }
        }
        public void Next()
        {
            int nextIndex = currentIndex + 1;
            if (nextIndex < characterPoints.Count)
            {
                Activate(nextIndex);
            }
        }

        public void Prev()
        {
            int prevIndex = currentIndex - 1;
            if (prevIndex >= 0)
            {
                Activate(prevIndex);
            }
        }

        public static void UnlockCharacter(string characterName)
        {
            Instance.NPCs[characterName].SetState("Unlocked");
        }

        public string GetIdentifier()
        {
            return "CharacterPanel";
        }

        public ISaveData GetSaveData()
        {
            CharacterPanelSaveData saveData = new CharacterPanelSaveData();
            saveData.States = new Dictionary<string, string>();
            foreach (var npc in NPCs)
            {
                saveData.States.Add(npc.Key, npc.Value.CardState);
            }
            return saveData;
        }

        public void SetSaveData(ISaveData saveData)
        {
            CharacterPanelSaveData saveDataCast = (CharacterPanelSaveData)saveData;
            foreach (var npc in NPCs)
            {
                if (saveDataCast.States.ContainsKey(npc.Key))
                {
                    npc.Value.SetState(saveDataCast.States[npc.Key]);
                }
            }
        }
    }
}
