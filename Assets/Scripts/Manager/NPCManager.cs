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
                agent?.Move(Instance.locations[roomName][index].position);
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
                Instance.StartCoroutine(TurnToLocationCoroutine(npc, roomName, index, speed));
            }
            else
            {
                Debug.Log("NPC " + name + " not found in NPC list");
            }
        }

        private static IEnumerator TurnToLocationCoroutine(GameObject npc, string roomName, int index,float speed = 1f)
        {
            while (npc.transform.rotation != Instance.locations[roomName][index].rotation)
            {
                npc.transform.rotation = Quaternion.RotateTowards(npc.transform.rotation, Instance.locations[roomName][index].rotation, speed);
                yield return null;
            }
        }

    }
}
