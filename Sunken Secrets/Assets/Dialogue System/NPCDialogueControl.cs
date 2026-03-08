using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCDialogueControl : MonoBehaviour
{
    public List<DialogueField> dialogueFields; // list of dialogue fields for this NPC
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerDialogueTrigger.interactionOccurance.AddListener(StartTalking);
        }
    }

    void StartTalking()
    {
        DialogueManager.NPCSpeaking.Invoke(dialogueFields);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerDialogueTrigger.interactionOccurance.RemoveListener(StartTalking);
        }
    }
}
