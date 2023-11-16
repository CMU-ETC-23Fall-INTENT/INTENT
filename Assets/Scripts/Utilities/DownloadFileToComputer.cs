using System.Runtime.InteropServices;
using UnityEngine;

namespace INTENT
{
    public static class DownloadFileHelper
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void downloadToFile(string content, string filename);
#endif

        public static void DownloadToFile(string content, string filename)
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
            System.IO.File.WriteAllText(filename, content);
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
            downloadToFile(content, filename);
#endif
        }

    }
}
