using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

namespace INTENT
{
    public class NPCManager : Singleton<NPCManager>
    {

        [SerializeField] public SerializableDictionary<string, GameObject> NPC = new SerializableDictionary<string, GameObject>();
        [SerializeField] private SerializableDictionary<string, List<Transform>> locations = new SerializableDictionary<string, List<Transform>>();

        private void Awake()
        {
            CheckAllNPCValid();
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
                Debug.Log("NPC " + name + " is now located in " + roomName + " at index " + index);
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
                Debug.Log("NPC " + name + " is moving to " + roomName + " at index " + index);
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

        private static IEnumerator TurnToCoroutine(GameObject npc, Quaternion qDest, float speed = 1f)
        {
            while (npc.transform.rotation != qDest)
            {
                npc.transform.rotation = Quaternion.RotateTowards(npc.transform.rotation, qDest, speed);
                yield return null;
            }
        }

        static Dictionary<KeyValuePair<string, string>, Coroutine> coroutineDict = new Dictionary<KeyValuePair<string, string>, Coroutine>();

        [YarnCommand("TurnToNPC")]
        public static void TurnToNPC(string nameFrom, string nameTo, float speed = 1f, bool continuously = false, bool toggle = false)
        {
            if (Instance.NPC.ContainsKey(nameFrom) && Instance.NPC.ContainsKey(nameTo))
            {
                GameObject npcFrom = Instance.NPC[nameFrom];
                GameObject npcTo = Instance.NPC[nameTo];
                if(!continuously)
                {
                    Quaternion qDest = Quaternion.FromToRotation(npcFrom.transform.position, npcTo.transform.position);
                    Instance.StartCoroutine(TurnToCoroutine(npcFrom, qDest, speed));
                }
                else
                {
                    var pair = new KeyValuePair<string, string>(nameFrom, nameTo);
                    if (!toggle && coroutineDict.ContainsKey(pair))
                    {
                        Instance.StopCoroutine(coroutineDict[pair]); //if exist, stop it
                        coroutineDict.Remove(pair);
                    }
                    else
                    {
                        if(coroutineDict.ContainsKey(pair))
                        {
                            Instance.StopCoroutine(coroutineDict[pair]); //if exist, stop it
                            coroutineDict.Remove(pair);
                        }
                        coroutineDict[pair] = Instance.StartCoroutine(TurnToNPCCoroutine(npcFrom, npcTo, speed));
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
    }
}
