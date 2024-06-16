using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

	public Dialogue dialogue;
	private bool dialogStarted;
    private bool dialogueClosed = true;
    public AudioSource? audioSource;

    public void TriggerDialogue ()
	{
        if (dialogueClosed)
        {
            Debug.Log("Opened dialogue");
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
            dialogStarted = true;
            dialogueClosed = false;
            audioSource.Play();
        }
    }

	public bool isDialogStarted()
	{
		return dialogStarted;
	}

    internal void NextDialogue()
    {
        Debug.Log("Next dialogue");
        if (!FindObjectOfType<DialogueManager>().DisplayNextSentence())
        {
            EndDialog();
        }
    }

    internal void EndDialog()
    {
        if (!dialogueClosed)
        {
            Debug.Log("Closed dialogue");
            dialogueClosed = true;
            dialogStarted = false;
            FindObjectOfType<DialogueManager>().EndDialogue();
            audioSource.Stop();     
        }
    }
}
