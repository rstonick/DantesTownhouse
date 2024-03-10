using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [TextArea(3, 10)]
    public string dialogueContent = "Orbs are $40 each to buy. You can sell them to me for $20 each.";

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
