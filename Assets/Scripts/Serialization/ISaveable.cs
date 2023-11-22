using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INTENT
{
    public abstract class ISaveData
    {

    }
    public interface ISaveable
    {
        public string GetIdentifier();
        public ISaveData GetSaveData();
        public void SetSaveData(ISaveData saveData);
    }
}
