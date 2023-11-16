using DA_Assets.SVGMeshUnity;
using System.Collections;
using System.Collections.Generic;
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
            DownloadFileHelper.DownloadToFile(saveDatasInJson, filename);
            Debug.Log("Saved INTENT save to file:" + filename);
        }

        public static void Load(string saveDatasInJson)
        {
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
        }
    }
}
