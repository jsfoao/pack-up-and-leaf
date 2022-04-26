using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    DialogueManager Manager; // level dialogue manager
    [SerializeField] Dialogue triggerDialogue; // the first dialogue that will play upon activation
    [SerializeField] bool expired = false; // trigger flag so it doesn't loop a million times, serialized for debug

    void Start()
    {
        Manager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>(); // get manager
    }

    private void OnTriggerEnter(Collider other)
    {
        if (expired)
        {
            // do absolutely nothing you big buffoon
        }
        else
        {
            expired = true; // sets flag
            Manager.getDialogue(triggerDialogue); // forwards the designated dialogue into the manager to play or enqueue
        }
        
    }
}
