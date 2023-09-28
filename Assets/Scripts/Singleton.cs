using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    private static bool applicationHasQuit = false;
    public static T Instance
    {
        get 
        {
            return GetInstance();
        }
    }

    public static T GetInstance()
    {
        if(instance != null)
            return instance;
        else
        {
            T TinScene = FindObjectOfType<T>();
            if (TinScene != null)
            {
                instance = TinScene;
            }
            else
            {
                Debug.Log("No instance of " + typeof(T) + " found in scene, creating one");
            }
            return instance;
        }
    }
    
}
