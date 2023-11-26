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

    [Serializable]
    public class SystemStatus: ISaveData
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
            return _status;
        }

        public void SetSaveData(ISaveData saveData)
        {
            _status = (SystemStatus)saveData;
        }

    }

}
