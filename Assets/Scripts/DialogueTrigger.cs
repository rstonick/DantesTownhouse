using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [TextArea(3, 10)]
    public string dialogueContent = "Hello, this is a sample dialogue. Press Enter to close.";
    private bool isPlayerNear = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (DialogueManager.instance != null) {
                DialogueManager.instance.StartDialogue(dialogueContent);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (DialogueManager.instance != null)
            {
                DialogueManager.instance.EndDialogue();
            }
        }
    }
}
