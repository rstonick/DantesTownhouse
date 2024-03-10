using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    public GameObject dialogueCanvas;
    public TextMeshProUGUI dialogueText;
    private bool isDialogueActive = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        dialogueCanvas.SetActive(false);
    }


    public void StartDialogue(string dialogue)
    {
        dialogueCanvas.SetActive(true);
        dialogueText.text = dialogue;
        isDialogueActive = true;
    }

    public void EndDialogue()
    {
        dialogueCanvas.SetActive(false);
        isDialogueActive = false;
    }

    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }
}