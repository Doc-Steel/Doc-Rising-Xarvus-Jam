using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    public GameObject npcInteractImage;
    public DialogueManager npcManager;
    public List<string> npcConvo = new List<string>();

    private bool isInside = false;

    private void Update()
    {
        if (isInside)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                npcManager.StartDialogue(npcConvo);
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInside = true;
            npcInteractImage.SetActive(true);
            Debug.Log("Player is inside NPC Range");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isInside = false;
        npcInteractImage.SetActive(false);
    }
    
}
