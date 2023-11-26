using DA_Assets.SVGMeshUnity;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using SFB;

namespace INTENT
{
    public class SaveStates
    {
        public bool HasName = false;
        public bool DoneTutorial = false;
        public string PlayerName = "Player";

        public Dictionary<string,ISaveData> SaveDatas = new Dictionary<string, ISaveData>();
    }

    public class SaveManager : Singleton<SaveManager>
    {
        public static SaveStates Savestates = new SaveStates();

        private void Awake()
        {
        }

        public static string SaveToJsonText()
        {
            Debug.Log("Saving INTENT save to json text");
            ISaveable[] saveables = FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>().ToArray();

            Savestates.SaveDatas.Clear();
            foreach (var saveable in saveables)
            {
                Savestates.SaveDatas.Add(saveable.GetIdentifier(),saveable.GetSaveData());
                // Implement the actual save logic (e.g., writing to a file or PlayerPrefs)
            }

            return JsonConvert.SerializeObject(Savestates, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.All
            });
        }

        public static void LoadFromJsonText(string jsonText)
        {
            GameManager.Instance.ResetGameState();
            Debug.Log("Loading INTENT save from json text");
            ISaveable[] saveables = FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>().ToArray();

            //if (string.IsNullOrEmpty(path)) return;
                
            Savestates = JsonConvert.DeserializeObject<SaveStates>(jsonText, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });

            foreach (var saveable in saveables)
            {
                ISaveData saveData;
                if (Savestates.SaveDatas.TryGetValue(saveable.GetIdentifier(), out saveData))
                {
                    saveable.SetSaveData(saveData);
                }
            }

            GameManager.Instance.GameStart();
        }
    }
}
