using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    public GameObject npcInteractImage;
    public DialogueManager npcManager;
    public List<string> npcConvo = new List<string>();

    private bool inInteractRange = false;

    private void Update()
    {
        if (!inInteractRange) { return; }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (npcManager.InDialogue)
            {
                npcManager.Next();
            }
            else
            {
                npcManager.StartDialogue(npcConvo);
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) { return; }
        inInteractRange = true;
        npcInteractImage.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) { return; }
        inInteractRange = false;
        npcInteractImage.SetActive(false);
    }

}
