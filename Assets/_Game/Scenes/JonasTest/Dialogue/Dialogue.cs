using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    [SerializeField] string descriptiveTag; //descriptive tag of the dialogue, debug purposes only
    public AudioClip voiceClip; //self-explanatory
    [TextArea] public string dialogue; //Written dialogue
    public Dialogue nextDialogue; //If the dialogue will lead into another dialogue
    public float timeLength = 0; //How many seconds the dialogue will be visible; This can be adjusted to fit the voice clip instead later

    void Start()
    {
        timeLength = 3 + Mathf.Ceil(Mathf.Sqrt(dialogue.Length) / 4); // I am not good at math
    }
}
