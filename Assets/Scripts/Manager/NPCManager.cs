using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Yarn.Unity;
using TMPro;

namespace INTENT
{
    public class NPCState
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Destination;
        public bool IsLookingAt;
        public string LookingTarget;
        public float LookingSpeed;
        public bool DialogueActivated;
        public string DialogueTitle;
        public bool UnLocked;
    }


    public class NPCManager : Singleton<NPCManager>, ISaveable
    {

        [SerializeField] public SerializableDictionary<string, GameObject> NPC = new SerializableDictionary<string, GameObject>();
        private List<string> unlockedNPC = new List<string>();
        [SerializeField] private SerializableDictionary<string, List<Transform>> locations = new SerializableDictionary<string, List<Transform>>();

        private void Awake()
        {
            CheckAllNPCValid();
            LoadNPCInteractionPoints();
        }

        public void CheckAllNPCValid()
        {
            foreach (KeyValuePair<string, GameObject> entry in NPC)
            {
                if(entry.Value == null)
                {
                    Debug.Log("NPC with name " + entry.Key + " is null");
                }
            }
        }

        public GameObject GetNPCByName(string name)
        {
            if (NPC.ContainsKey(name))
                return NPC[name];
            else
            {
                String debugStr = String.Format("NPC {0} not found in NPC list", name);
                Debug.Log(debugStr);
                return null;
            }
        }
        public void LoadNPCInteractionPoints()
        {
            foreach (KeyValuePair<string, GameObject> entry in NPC)
            {
                if (entry.Value)
                {
                    UltimateInteractionPoint interactionPoint = GetNPCInteractionPoint(entry.Key);
                    if(interactionPoint == null)
                    {
                        continue;
                    }
                    if(interactionPoint.IsAvailable)
                    {
                        interactionPoint.ToggleAvailable(true, true);
                    }
                    else
                    {
                        interactionPoint.ToggleAvailable(false, true);;
                    }
                }
            }
        }
        public UltimateInteractionPoint GetNPCInteractionPoint(string npcName)
        {
            if (NPC.ContainsKey(npcName))
            {
                GameObject npc = NPC[npcName];
                UltimateInteractionPoint interactionPoint = npc.GetComponentInChildren<UltimateInteractionPoint>(true);
                if (interactionPoint != null)
                {
                    return interactionPoint;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        [YarnCommand("GetNPCLocation")]
        public void GetNPCLocation(string name)
        {
            if (NPC.ContainsKey(name))
            {
                GameObject npc = NPC[name];
                Vector3 location = npc.transform.position;
                String debugStr = String.Format("NPC {0} is now located in ({1:F2}, {2:F2}, {3:F2})", name, location.x, location.y, location.z);
                Debug.Log(debugStr);
            }
            else
            {
                String debugStr = String.Format("NPC {0} not found in NPC list", name);
                Debug.Log(debugStr);
            }
        }

        [YarnCommand("TeleportToLocation")]
        public void TeleportToLocation(string name, string roomName, int index)
        {
            if(NPC.ContainsKey(name))
            {
                GameObject npc = NPC[name];
                UnityEngine.AI.NavMeshAgent agent = npc.GetComponent<UnityEngine.AI.NavMeshAgent>();
                agent?.Warp(locations[roomName][index].position);
                npc.transform.rotation = locations[roomName][index].rotation;
                npc.GetComponent<AgentPositionKeeper>()?.SetPositionToKeep(locations[roomName][index].position);
                //Debug.Log("NPC " + name + " is now located in " + roomName + " at index " + index);
            }
            else
            {
                Debug.Log("NPC " + name + " not found in NPC list");
            }
        }

        [YarnCommand("NPCEmote")]
        public void NPCEmote(string name, string emote)
        {
            if(NPC.ContainsKey(name))
            {
                GameObject npc = NPC[name];
                Debug.Log("NPC " + name + " is now " + emote);
            }
            else
            {
                Debug.Log("NPC " + name + " not found in NPC list");
            }
        }

        [YarnCommand("MoveToLocation")]
        public static void MoveToLocation(string name, string roomName, int index)
        {
            if(Instance.NPC.ContainsKey(name))
            {
                GameObject npc = Instance.NPC[name];
                UnityEngine.AI.NavMeshAgent agent = npc.GetComponent<UnityEngine.AI.NavMeshAgent>();
                agent?.SetDestination(Instance.locations[roomName][index].position);
                npc.GetComponent<AgentPositionKeeper>()?.SetPositionToKeep(Instance.locations[roomName][index].position);
                npc.GetComponent<AgentPositionKeeper>()?.SetDestinTransform(Instance.locations[roomName][index]);
                //Debug.Log("NPC " + name + " is moving to " + roomName + " at index " + index);
            }
            else
            {
                Debug.Log("NPC " + name + " not found in NPC list");
            }
        }

        [YarnCommand("TurnToLocation")]
        public static void TurnToLocation(string name, string roomName, int index, float speed = 1f)
        {
            if(Instance.NPC.ContainsKey(name))
            {
                GameObject npc = Instance.NPC[name];
                Instance.StartCoroutine(TurnToCoroutine(npc, Instance.locations[roomName][index].rotation, speed));
            }
            else
            {
                Debug.Log("NPC " + name + " not found in NPC list");
            }
        }
        public void RotateToDestination(GameObject npc, Transform destin, float speed)
        {
            StartCoroutine(TurnToCoroutine(npc, destin.rotation, speed));
        }
        private static IEnumerator TurnToCoroutine(GameObject npc, Quaternion qDest, float speed = 1f, float timeLimit = 3f)
        {
            float timer = 0f;
            while (npc.transform.rotation != qDest && timer < timeLimit)
            {
                timer += Time.deltaTime;
                npc.transform.rotation = Quaternion.RotateTowards(npc.transform.rotation, qDest, speed);
                yield return null;
            }
        }

        public class TurnToData
        {
            public string Target;
            public float Speed;
            public Coroutine Coroutine;
        }
        //static Dictionary<KeyValuePair<string, string>, Coroutine> coroutineDict = new Dictionary<KeyValuePair<string, string>, Coroutine>();
        static Dictionary<string, TurnToData> coroutineDict = new Dictionary<string, TurnToData>();

        [YarnCommand("TurnToNPC")]
        public static void TurnToNPC(string nameFrom, string nameTo, float speed = 1f, bool continuously = false, bool toggle = false)
        {
            if (Instance.NPC.ContainsKey(nameFrom) && Instance.NPC.ContainsKey(nameTo))
            {
                GameObject npcFrom = Instance.NPC[nameFrom];
                GameObject npcTo = Instance.NPC[nameTo];
                if(!continuously) // turn to only once
                {
                    Quaternion qDest = Quaternion.FromToRotation(npcFrom.transform.position, npcTo.transform.position);
                    Instance.StartCoroutine(TurnToCoroutine(npcFrom, qDest, speed));
                }
                else // continuously turn to
                {
                    if (!toggle && coroutineDict.ContainsKey(nameFrom)) // toggle off
                    {
                        Instance.StopCoroutine(coroutineDict[nameFrom].Coroutine); //if exist, stop it
                        coroutineDict.Remove(nameFrom);
                    }
                    else //toggle on
                    {
                        if(coroutineDict.ContainsKey(nameFrom))
                        {
                            Instance.StopCoroutine(coroutineDict[nameFrom].Coroutine); //if exist, stop it
                            coroutineDict.Remove(nameFrom);
                        }
                        coroutineDict[nameFrom] = new TurnToData() { Target = nameTo, Speed = speed, Coroutine = Instance.StartCoroutine(TurnToNPCCoroutine(npcFrom, npcTo, speed)) };
                    }

                }
            }
            else
            {
                string str = string.Format("NPC {0} or {1} not found in NPC list", nameFrom, nameTo);
                Debug.Log(str);
            }
        }

        private static IEnumerator TurnToNPCCoroutine(GameObject npcFrom, GameObject npcTo, float speed = 1f)
        {
            while (true)
            {
                Vector3 fromDir = new Vector3(0, 0, 1f);
                Vector3 toDir = npcTo.transform.position - npcFrom.transform.position;
                Quaternion qDest = Quaternion.FromToRotation(fromDir, toDir);
                npcFrom.transform.rotation = Quaternion.RotateTowards(npcFrom.transform.rotation, qDest, speed);
                yield return null;
            }
        }
        
        [YarnCommand("ChangeNPCDialog")]
        public static void ChangeNPCDialog( string npcName, bool enable, string dialogueTitle = null)
        {
            GameObject npc = Instance.GetNPCByName(npcName);
            UltimateInteractionPoint interactionPoint;
            if(npc != null)
            {
                interactionPoint = Instance.GetNPCInteractionPoint(npcName);
                InteractionBase interactionBase;
                if(!enable)
                {
                    Debug.Log("NPC: " + npcName + " disable");
                    interactionPoint.ToggleAvailable(false, true);;
                    return;
                }
                else
                {
                    Debug.Log("NPC: " + npcName + " enable");
                    interactionPoint.ToggleAvailable(true, true);
                }

                if(interactionPoint != null)
                {
                    interactionBase = interactionPoint.GetCurrentInteraction();
                    if(interactionBase != null)
                    {
                        if(dialogueTitle != null)
                        {
                            interactionBase.ChangeDialogTitle(dialogueTitle);
                            Debug.Log("NPC " + npcName + " dialogue activated, and changed to " + dialogueTitle);
                        }
                        else
                        {
                            Debug.Log("NPC " + npcName + " dialogue activated");
                        }
                        
                    }
                }
            }

        }
        public void UnLockNPCNametag(string npcName)
        {
            GameObject npc = GetNPCByName(npcName);
            if(npc != null)
            {
                unlockedNPC.Add(npcName);
                TextMeshPro nameTag = npc.transform.Find("NameTag").Find("NameText").GetComponent<TextMeshPro>();
                if(nameTag != null)
                    nameTag.text = npcName;
                else
                    Debug.Log("NPC " + npcName + " name tag not found");
            }
        }

        #region Save and Load
        public string GetIdentifier()
        {
            return "NPCManager";
        }

        public class NPCManagerSaveData:ISaveData
        {
            public Dictionary<string, NPCState> NPCs = new Dictionary<string, NPCState>();
        }

        public ISaveData GetSaveData()
        {
            NPCManagerSaveData saveData = new NPCManagerSaveData();

            foreach (KeyValuePair<string, GameObject> entry in NPC)
            {
                if (entry.Value)
                {
                    NPCState npcState = new NPCState();
                    npcState.Position = entry.Value.transform.position;
                    npcState.Rotation = entry.Value.transform.rotation;
                    npcState.Destination = entry.Value.GetComponent<UnityEngine.AI.NavMeshAgent>().destination;
                    if(Instance.GetNPCInteractionPoint(entry.Key) != null)
                    {
                        npcState.DialogueActivated = Instance.GetNPCInteractionPoint(entry.Key).IsAvailable;
                        npcState.DialogueTitle = Instance.GetNPCInteractionPoint(entry.Key).GetCurrentInteraction().GetDialogTitle();
                    }
                    

                    if(coroutineDict.ContainsKey(entry.Key))
                    {
                        npcState.IsLookingAt = true;
                        npcState.LookingTarget = coroutineDict[entry.Key].Target;
                        npcState.LookingSpeed = coroutineDict[entry.Key].Speed;
                    }

                    if(unlockedNPC.Contains(entry.Key))
                    {
                        npcState.UnLocked = true;
                    }
                    else
                    {
                        npcState.UnLocked = false;
                    }

                    saveData.NPCs[entry.Key] = npcState;
                }
            }
            return saveData;
        }

        public void SetSaveData(ISaveData saveData)
        {
            NPCManagerSaveData _saveData = saveData as NPCManagerSaveData;
            unlockedNPC.Clear();
            foreach (KeyValuePair<string, NPCState> entry in _saveData.NPCs)
            {
                if (NPC.ContainsKey(entry.Key))
                {
                    NPCState npcState = entry.Value;
                    GameObject npc = NPC[entry.Key];
                    npc.GetComponent<UnityEngine.AI.NavMeshAgent>().destination = npcState.Destination;
                    npc.GetComponent<UnityEngine.AI.NavMeshAgent>().Warp(npcState.Position);
                    npc.transform.position = npcState.Position;
                    npc.transform.rotation = npcState.Rotation;
                    npc.GetComponent<AgentPositionKeeper>()?.SetPositionToKeep(npcState.Destination);
                    if(Instance.GetNPCInteractionPoint(entry.Key) != null)
                    {
                        Instance.GetNPCInteractionPoint(entry.Key).ToggleAvailable(npcState.DialogueActivated, true);
                        Instance.GetNPCInteractionPoint(entry.Key).GetCurrentInteraction().ChangeDialogTitle(npcState.DialogueTitle);
                    }
                    

                    if (npcState.IsLookingAt)
                    {
                        TurnToNPC(npc.name, npcState.LookingTarget, npcState.LookingSpeed, true, true);
                    }
                    if(npcState.UnLocked)
                    {
                        UIManager.UnlockCharacter(npc.name, false);
                    }
                }
            }
        }
        #endregion

    }
}
