using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SFB;
using INTENT;

[RequireComponent(typeof(Button))]
public class SaveFileButton : MonoBehaviour, IPointerDownHandler {

#if UNITY_WEBGL && !UNITY_EDITOR
    //
    // WebGL
    //
    [DllImport("__Internal")]
    private static extern void DownloadFile(string gameObjectName, string methodName, string filename, byte[] byteArray, int byteArraySize);

    // Browser plugin should be called in OnPointerDown.
    public void OnPointerDown(PointerEventData eventData) {
        var bytes = Encoding.UTF8.GetBytes(SaveManager.SaveToJsonText());
        string filename = "INTENT-Save-" + System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".json";
        DownloadFile(gameObject.name, "OnFileDownload", filename, bytes, bytes.Length);
    }

    // Called from browser
    public void OnFileDownload() {
        //output.text = "File Successfully Downloaded";
    }
#else
    //
    // Standalone platforms & editor
    //
    public void OnPointerDown(PointerEventData eventData) { }

    // Listen OnClick event in standlone builds
    void Start() {
        var button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    public void OnClick() {
        string filename = "INTENT-Save-" + System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        var path = StandaloneFileBrowser.SaveFilePanel("Please choose where you want to save", "", filename, "json");
        if (!string.IsNullOrEmpty(path)) {
            File.WriteAllText(path, SaveManager.SaveToJsonText());
        }
    }
#endif
}
