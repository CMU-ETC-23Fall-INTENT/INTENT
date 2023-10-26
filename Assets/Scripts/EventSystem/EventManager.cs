using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INTENT
{
    public class EventManager : Singleton<EventManager>
    {
        public TaskEvents TaskEvents;
        public PlayerEvents PlayerEvents;
        // Start is called before the first frame update
        void Awake()
        {
            TaskEvents = new TaskEvents();
            PlayerEvents = new PlayerEvents();
        }

    }
}

