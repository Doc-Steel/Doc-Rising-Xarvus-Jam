using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogPanel;
    //public GameObject interactCanvas;
    public TextMeshProUGUI dialogText;

    public bool InDialogue { get; private set; } = false;

    private List<string> conversation;
    public int convoIndex;

    public void Start()
    {
        dialogPanel.SetActive(false);
        //interactCanvas.SetActive(false);
    }

    public void StartDialogue(List<string> convo)
    {
        InDialogue = true;
        convoIndex = 0;
        conversation = new List<string>(convo);
        dialogPanel.SetActive(true);

        UpdateText();
    }

    public void StopDialogue()
    {
        InDialogue = false;
        convoIndex = 0;
        dialogPanel.SetActive(false);
    }

    public void UpdateText()
    {
        dialogText.text = conversation[convoIndex];
    }

    public void Next()
    {
        if (convoIndex < conversation.Count - 1)
        {
            convoIndex++;
            UpdateText();
        }
        else
        {
            StopDialogue();
        }
    }
}
