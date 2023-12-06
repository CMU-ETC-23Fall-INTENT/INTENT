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
        public bool IsCharacterButtonNewBadgeActive = false;
        public Dictionary<string, bool> GameObjectsActiveState = new Dictionary<string, bool>();
    }

    public class CharacterPanelControl : MonoBehaviour, ISaveable
    {
        [SerializeField] SerializableDictionary<string, CharacterPanelPerNPCControl> NPCs;
        [SerializeField] private GameObject NPCRoot;
        [SerializeField] private List<GameObject> characterPoints;
        [SerializeField] private NavBarControl characterNavBar;
        [SerializeField] private Button characterButton;
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

        public void UnlockCharacter(string characterName, bool showNewBadge)
        {
            NPCs[characterName].SetState("Unlocked");
            if(showNewBadge)
                characterButton.gameObject.GetComponent<ButtonNewBadgeControl>().ShowNewBadge();
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
            saveData.IsCharacterButtonNewBadgeActive = characterButton.gameObject.GetComponent<ButtonNewBadgeControl>().IsNewBadgeActive();
            GetGameObjectsActiveState(NPCRoot, NPCRoot, saveData.GameObjectsActiveState);
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
            characterButton.gameObject.GetComponent<ButtonNewBadgeControl>().SetNewBadgeActive(saveDataCast.IsCharacterButtonNewBadgeActive);
            SetGameObjectsActiveState(NPCRoot, NPCRoot, saveDataCast.GameObjectsActiveState);
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
                GetGameObjectsActiveState(child.gameObject, root, gameObjectsActiveState);
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
