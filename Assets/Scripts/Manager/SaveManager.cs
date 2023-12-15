using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using SFB;
using UnityEngine.SceneManagement;

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

        public static string tempJsonText = "";

        private void Awake()
        {
        }

        public static string SaveToJsonText()
        {
            Debug.Log("Saving INTENT save to json text");
            ISaveable[] saveables = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveable>().ToArray();

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

        public static IEnumerator OnSceneLoaded()
        {
            yield return new WaitForEndOfFrame();
            GameManager.Instance.ResetGameState();
            Debug.Log("Loading INTENT save from json text");
            ISaveable[] saveables = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveable>().ToArray();

            //if (string.IsNullOrEmpty(path)) return;

            Savestates = JsonConvert.DeserializeObject<SaveStates>(tempJsonText, new JsonSerializerSettings
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
            tempJsonText = "";
            GameManager.Instance.GameStart();
        }

        public static void LoadFromJsonText(string jsonText)
        {
            tempJsonText = jsonText;
            SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }

        private static void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
            Instance.StartCoroutine(OnSceneLoaded());
        }
    }
}
