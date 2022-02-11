using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] int currentDialogue; // the iteration of dialogue that is currently being displayed
    [SerializeField] List<Dialogue> dialogueList = new List<Dialogue>(); // the list of dialogue that the manager goes through
    [SerializeField] Queue<Dialogue> waitingQueue = new Queue<Dialogue>(); // if another trigger plays while another dialogue is currently playing, enqueue the dialogue
    [SerializeField] Text dialogueText; // the text object
    [SerializeField] AudioSource audioSource;
    bool isTalking = false;

    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void getDialogue(Dialogue dialogue) // gathers dialogue from a trigger object (see DialogueTrigger)
    {
        if (isTalking) // if another dialogue is already playing, enqueue it for later
        {
            waitingQueue.Enqueue(dialogue);
        }
        else
        {
            dialogueList.Add(dialogue);
            refreshDialogue();
        }
    }
    void refreshDialogue() // this plays the dialogue gathered from getDialogue()
    {
        dialogueText.color = new Color(dialogueText.color.r, dialogueText.color.g, dialogueText.color.b, 255); // visibility
        isTalking = true;
        audioSource.PlayOneShot(dialogueList[currentDialogue].voiceClip);
        dialogueText.text = dialogueList[currentDialogue].dialogue;
        StartCoroutine(nextDialogue());
    }

    IEnumerator nextDialogue()
    {
        yield return new WaitForSeconds(dialogueList[currentDialogue].timeLength); // delay into the next dialogue or the end, time is based on how long the previous dialogue is
        if (dialogueList[currentDialogue].nextDialogue != null) // if the current dialogue leads into another (see Dialogue)
        {
            dialogueList.Add(dialogueList[currentDialogue].nextDialogue);
            currentDialogue++;
            refreshDialogue();
        }
        else // if it ends, end the dialogue
        {
            closeDialogue();
            if (waitingQueue.Count != 0) // but if another dialogue is enqueued, play it shortly after
            {
                yield return new WaitForSeconds(0.5f);
                dialogueList.Add(waitingQueue.Dequeue());
                refreshDialogue();
            }
        }
        
    }

    void closeDialogue() // quits the dialogue processing by clearing variables and making the text object invisible
    {
        isTalking = false;
        dialogueText.color = new Color(dialogueText.color.r, dialogueText.color.g, dialogueText.color.b, 0);
        dialogueList.Clear();
        currentDialogue = 0;
    }
}
