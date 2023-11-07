using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

namespace INTENT
{
    public class LoggingManager : Singleton<LoggingManager>
    {
        public struct INTENTLog
        {
            public string LogType;
            public string Message;
            public float TimeFromBeginning;
            public float TimeFromLastSameEvent;
            public float TimeFromLastEvent;
        }

        public List<INTENTLog> INTENTLogs = new List<INTENTLog>();
        private Dictionary<string, float> lastLogTimePerType = new Dictionary<string, float>();
        private float lastLogTime = 0f;

        void Awake()
        {
            DontDestroyOnLoad(this);
        }

        [YarnCommand("Log")]
        public void Log(string type, string message)
        {
            INTENTLogs.Add(new INTENTLog
            {
                LogType = type,
                Message = ProcessMessage(message),
                TimeFromBeginning = Time.time,
                TimeFromLastSameEvent = lastLogTimePerType.ContainsKey(type) ? Time.time - lastLogTimePerType[type] : 0f,
                TimeFromLastEvent = Time.time - lastLogTime
            });
            lastLogTime = Time.time;
            lastLogTimePerType[type] = lastLogTime;
        }

        private string ListToCSV()
        {
            string csv = "TimeFromBeginning,TimeFromLastSameEvent,TimeFromLastEvent,LogType,Message\n";
            foreach (var log in INTENTLogs)
            {
                csv += string.Format("{0:0.00}\t,{1:0.00}\t,{2:0.00}\t,{3},{4}\n", log.TimeFromBeginning, log.TimeFromLastSameEvent, log.TimeFromLastEvent, log.LogType, log.Message);
            }
            return csv;
        }


        // Save to json, with filename to the format similar to 2023-11-03-23-41-00.json
        public void Save()
        {
            string filename = "INTENT-Log-" + System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".csv";
            DownloadFileHelper.DownloadToFile(ListToCSV(), filename);
            Debug.Log("Saved INTENT logs to file:" + filename);
        }

        //filter and replace some signs not supported by csv
        public string ProcessMessage(string message)
        {
            string result = message;
            result = result.Replace("\n", " ").Replace("\"", "\"\"");
            //if comma(,) included, wrap with double quotes
            if (result.Contains(","))
            {
                result = "\"" + result + "\"";
            }
            return result;
        }
    }
}
