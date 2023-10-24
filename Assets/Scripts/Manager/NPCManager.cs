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
                npc.transform.position = locations[roomName][index].position;
                Debug.Log("NPC " + name + " is now located in " + roomName + " at index " + index);
            }
            else
            {
                Debug.Log("NPC " + name + " not found in NPC list");
            }
        }
    }
}
