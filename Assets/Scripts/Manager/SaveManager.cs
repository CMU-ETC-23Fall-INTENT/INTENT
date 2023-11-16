using DA_Assets.SVGMeshUnity;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace INTENT
{
    public class SaveManager : Singleton<SaveManager>
    {
        private Dictionary<string, ISaveable> saveables = new Dictionary<string, ISaveable>();

        public static void RegisterSaveable(ISaveable saveable, string identifier)
        {
            if (!Instance.saveables.ContainsKey(identifier))
            {
                Instance.saveables.Add(identifier, saveable);
            }
        }

        public static void Save()
        {
            Dictionary<string, Dictionary<string, string>> saveDatas = new Dictionary<string, Dictionary<string, string>>();
            foreach (var saveable in Instance.saveables)
            {
                saveDatas.Add(saveable.Key,saveable.Value.GetSaveData());
                // Implement the actual save logic (e.g., writing to a file or PlayerPrefs)
            }
            var saveDatasInJson = JsonUtility.ToJson(saveDatas);
            string filename = "INTENT-Save-" + System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".json";

            var path = SFB.StandaloneFileBrowser.SaveFilePanel("Saving to ...", "", filename, "json");
            if (!string.IsNullOrEmpty(path))
            {
                File.WriteAllText(path, saveDatasInJson);
            }

            //DownloadFileHelper.DownloadToFile(saveDatasInJson, filename);
            Debug.Log("Saved INTENT save to file:" + path);
        }

        public static void Load()
        {
            var paths = SFB.StandaloneFileBrowser.OpenFilePanel("Loading from ...", "", "json", false);
            if (paths.Length <= 0) return;

            var path = paths[0];
            if (string.IsNullOrEmpty(path)) return;

            string saveDatasInJson = File.ReadAllText(path);

            Dictionary<string, Dictionary<string, string>> saveDatas = JsonUtility.FromJson<Dictionary<string, Dictionary<string, string>>>(saveDatasInJson);

            foreach (var saveable in Instance.saveables)
            {
                var identifier = saveable.Key;
                Dictionary<string, string> saveData;
                if (saveDatas.TryGetValue(identifier, out saveData))
                {
                    saveable.Value.SetSaveData(saveData);
                }
            }
            Debug.Log("Loaded INTENT save from file:" + path);
        }
    }
}
