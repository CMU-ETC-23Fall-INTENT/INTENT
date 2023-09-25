using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Dependency: DS Dialogue
[RequireComponent(typeof(DS.DSDialogue))]
public class TryGetDialogue : MonoBehaviour
{
    private DS.DSDialogue dsDialogue;

    // Start is called before the first frame update
    void Start()
    {
        dsDialogue = GetComponent<DS.DSDialogue>();
        Debug.Log(dsDialogue.dialogue.Text);
        Debug.Log(dsDialogue.dialogue.Choices);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
