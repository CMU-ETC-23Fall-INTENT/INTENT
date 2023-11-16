using DA_Assets.SVGMeshUnity;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

namespace INTENT
{
    public class SaveStates
    {
        public bool HasName = false;
        public bool DoneTutorial = false;
        public string PlayerName = "Player";

        public Dictionary<string, Dictionary<string, string>> SaveDatas = new Dictionary<string, Dictionary<string, string>>();
    }

    public class SaveManager : Singleton<SaveManager>
    {
        public static SaveStates Savestates = new SaveStates();

        private void Awake()
        {
            //Load(); //TODO: A starting panel for new / load game
        }

        public static void Save()
        {
            ISaveable[] saveables = FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>().ToArray();

            foreach (var saveable in saveables)
            {
                Savestates.SaveDatas.Add(saveable.GetIdentifier(),saveable.GetSaveData());
                // Implement the actual save logic (e.g., writing to a file or PlayerPrefs)
            }

            string filename = "INTENT-Save-" + System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");

            var path = SFB.StandaloneFileBrowser.SaveFilePanel("Saving to ...", "", filename, "json");
            if (!string.IsNullOrEmpty(path))
            {
                Serialize(path, Savestates);
            }

            //DownloadFileHelper.DownloadToFile(saveDatasInJson, filename);
            Debug.Log("Saved INTENT save to file:" + path);
        }

        public static void Load()
        {
            ISaveable[] saveables = FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>().ToArray();

            var paths = SFB.StandaloneFileBrowser.OpenFilePanel("Loading from ...", "", "json", false);
            if (paths.Length <= 0) return;

            var path = paths[0];
            if (string.IsNullOrEmpty(path)) return;

            Savestates = Deserialize<SaveStates>(path);

            foreach (var saveable in saveables)
            {
                Dictionary<string, string> saveData;
                if (Savestates.SaveDatas.TryGetValue(saveable.GetIdentifier(), out saveData))
                {
                    saveable.SetSaveData(saveData);
                }
            }
            Debug.Log("Loaded INTENT save from file:" + path);
        }
        public static void Serialize<T>(string fileName, T value)
        {
            JsonSerializer jsonSerializer = new JsonSerializer();

            using (StreamWriter streamWriter = new StreamWriter(fileName))
            using (JsonWriter jsonTextWriter = new JsonTextWriter(streamWriter))
            {
                jsonSerializer.Serialize(jsonTextWriter, value);
            }
        }

        public static T Deserialize<T>(string fileName)
        {
            JsonSerializer jsonSerializer = new JsonSerializer();
            using (StreamReader streamReader = new StreamReader(fileName))
            using (JsonReader jsonTextReader = new JsonTextReader(streamReader))
            {
                return jsonSerializer.Deserialize<T>(jsonTextReader);
            }
        }
    }
}
