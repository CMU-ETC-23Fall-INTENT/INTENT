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
    }
}
