using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using INTENT;
using UnityEngine;
using Yarn.Unity;


public class EpisodeFolder: MonoBehaviour
{
    public bool AlwaysLoad;
    public Episode Episode;
    public GameObject InteractionFolder;
    public GameObject LocationFolder;

    private void Awake()
    {
        GetFolders();
    }
    private void OnValidate()
    {
        GetFolders();
    }
    private void GetFolders()
    {
        foreach (Transform child in transform)
        {
            if(child.CompareTag("InteractionFolder"))
                InteractionFolder = child.gameObject;
            else if(child.CompareTag("LocationFolder"))
                LocationFolder = child.gameObject;
        }

    }
}