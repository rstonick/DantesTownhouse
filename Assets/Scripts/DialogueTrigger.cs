using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Table1"))
        {
            if (DialogueManager.instance != null) {
                DialogueManager.instance.StartDialogue("Risk of 20% loss. Reward is 1:1. Press Enter to invest.");
            }
        } else if (other.CompareTag("Table2"))
        {
            if (DialogueManager.instance != null) {
                DialogueManager.instance.StartDialogue("Risk of 50% loss. Reward is 2:1. Press Enter to invest.");
            }
        } else if (other.CompareTag("Table3"))
        {
            if (DialogueManager.instance != null) {
                DialogueManager.instance.StartDialogue("Risk of 80% loss. Reward is 5:1. Press Enter to invest.");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Table1") || other.CompareTag("Table2") || other.CompareTag("Table3"))
        {
            if (DialogueManager.instance != null)
            {
                DialogueManager.instance.EndDialogue();
            }
        }
    }
}
