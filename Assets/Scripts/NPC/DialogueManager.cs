using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogPanel;
    //public GameObject interactCanvas;
    public TextMeshProUGUI dialogText;

    public bool inDialogue;

    private List<string> conversation;
    public int convoIndex;



    public void Start()
    {
        dialogPanel.SetActive(false);
        //interactCanvas.SetActive(false);
    }

    public void Update()
    {
        CheckEnd();

    }

    public void StartDialogue(List<string> convo)
    {

        convoIndex = 0;
        conversation = new List<string>(convo);
        dialogPanel.SetActive(true);

        ShowText();
    }

    public void StopDialog()
    {


        convoIndex = 0;
        dialogPanel.SetActive(false);

    }

    public void ShowText()
    {
        dialogText.text = conversation[convoIndex];
    }

    public void Next()
    {
        if (convoIndex < conversation.Count)
        {
            convoIndex++;
            dialogText.text = "";
            ShowText();
        }
    }

    public void CheckEnd()
    {
        if (convoIndex == conversation?.Count)
        {
            StopDialog();
        }
    }
}
