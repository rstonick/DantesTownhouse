using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [TextArea(3, 10)]
    public string dialogueContent = "Hello, this is a sample dialogue. Press Enter to close.";
    private bool isPlayerNear = false;

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.Return))
        {
            if (DialogueManager.instance != null)
            {
                if (DialogueManager.instance.IsDialogueActive())
                {
                    DialogueManager.instance.EndDialogue();
                }
                else
                {
                    DialogueManager.instance.StartDialogue(dialogueContent);
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
        }
    }
}
