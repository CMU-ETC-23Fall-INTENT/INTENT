using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace INTENT
{
    [Serializable]
    public class SystemStatusItem<T>
    {
        public T Value;
        public UnityEvent<T> OnValueChanged;
    }

    public class SystemStatusSaveData : ISaveData
    {
        public Dictionary<string, bool> StatusItemsBool = new Dictionary<string, bool>();
        public Dictionary<string, int> StatusItemsInt = new Dictionary<string, int>();
        public Dictionary<string, float> StatusItemsFloat = new Dictionary<string, float>();
        public Dictionary<string, string> StatusItemsString = new Dictionary<string, string>();
    };


    [Serializable]
    public class SystemStatus
    {
        public SerializableDictionary<string, SystemStatusItem<bool>> StatusItemsBool;
        public SerializableDictionary<string, SystemStatusItem<int>> StatusItemsInt;
        public SerializableDictionary<string, SystemStatusItem<float>> StatusItemsFloat;
        public SerializableDictionary<string, SystemStatusItem<string>> StatusItemsString;
    };

    public class StatusManager : Singleton<StatusManager>, ISaveable
    {
        [SerializeField] SystemStatus _status;

        public static void SetStatus(string statusName, object value)
        {
            if (value is bool && Instance._status.StatusItemsBool.ContainsKey(statusName))
            {
                Instance._status.StatusItemsBool[statusName].Value = (bool)(object)value;
                Instance._status.StatusItemsBool[statusName].OnValueChanged.Invoke((bool)(object)value);
            }
            else if (value is int && Instance._status.StatusItemsInt.ContainsKey(statusName))
            {
                Instance._status.StatusItemsInt[statusName].Value = (int)(object)value;
                Instance._status.StatusItemsInt[statusName].OnValueChanged.Invoke((int)(object)value);
            }
            else if (value is float && Instance._status.StatusItemsFloat.ContainsKey(statusName))
            {
                Instance._status.StatusItemsFloat[statusName].Value = (float)(object)value;
                Instance._status.StatusItemsFloat[statusName].OnValueChanged.Invoke((float)(object)value);
            }
            else if (value is string && Instance._status.StatusItemsString.ContainsKey(statusName))
            {
                Instance._status.StatusItemsString[statusName].Value = (string)(object)value;
                Instance._status.StatusItemsString[statusName].OnValueChanged.Invoke((string)(object)value);
            }
            else
            {
                throw new Exception("StatusManager: Status not found: " + statusName);
            }

        }

        public static T GetStatus<T>(string statusName)
        {
            if (typeof(T) == typeof(bool) && Instance._status.StatusItemsBool.ContainsKey(statusName))
            {
                return (T)(object)Instance._status.StatusItemsBool[statusName].Value;
            }
            else if (typeof(T) == typeof(int) && Instance._status.StatusItemsInt.ContainsKey(statusName))
            {
                return (T)(object)Instance._status.StatusItemsInt[statusName].Value;
            }
            else if (typeof(T) == typeof(float) && Instance._status.StatusItemsFloat.ContainsKey(statusName))
            {
                return (T)(object)Instance._status.StatusItemsFloat[statusName].Value;
            }
            else if (typeof(T) == typeof(string) && Instance._status.StatusItemsString.ContainsKey(statusName))
            {
                return (T)(object)Instance._status.StatusItemsString[statusName].Value;
            }
            else
            {
                throw new Exception("StatusManager: Status not found: " + statusName);
            }
        }


        public string GetIdentifier()
        {
            return "StatusManager";
        }

        public ISaveData GetSaveData()
        {
            SystemStatusSaveData _status = new SystemStatusSaveData();
            foreach (var item in Instance._status.StatusItemsBool)
            {
                _status.StatusItemsBool.Add(item.Key, item.Value.Value);
            }
            foreach (var item in Instance._status.StatusItemsInt)
            {
                _status.StatusItemsInt.Add(item.Key, item.Value.Value);
            }
            foreach (var item in Instance._status.StatusItemsFloat)
            {
                _status.StatusItemsFloat.Add(item.Key, item.Value.Value);
            }
            foreach (var item in Instance._status.StatusItemsString)
            {
                _status.StatusItemsString.Add(item.Key, item.Value.Value);
            }
            return _status;
        }

        public void SetSaveData(ISaveData saveData)
        {
            SystemStatusSaveData _newStatus = (SystemStatusSaveData)saveData;
            foreach (var item in _newStatus.StatusItemsBool)
            {
                Instance._status.StatusItemsBool[item.Key].Value = item.Value;
                Instance._status.StatusItemsBool[item.Key].OnValueChanged.Invoke(item.Value);
            }
            foreach (var item in _newStatus.StatusItemsInt)
            {
                Instance._status.StatusItemsInt[item.Key].Value = item.Value;
                Instance._status.StatusItemsInt[item.Key].OnValueChanged.Invoke(item.Value);
            }
            foreach (var item in _newStatus.StatusItemsFloat)
            {
                Instance._status.StatusItemsFloat[item.Key].Value = item.Value;
                Instance._status.StatusItemsFloat[item.Key].OnValueChanged.Invoke(item.Value);
            }
            foreach (var item in _newStatus.StatusItemsString)
            {
                Instance._status.StatusItemsString[item.Key].Value = item.Value;
                Instance._status.StatusItemsString[item.Key].OnValueChanged.Invoke(item.Value);
            }
        }

    }

}
