using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INTENT
{
    public interface ISaveable
    {
        public Dictionary<string, string> GetSaveData();
        public void SetSaveData(Dictionary<string, string> saveData);

        //TODO: inject itself into save manager
        
    }
}
