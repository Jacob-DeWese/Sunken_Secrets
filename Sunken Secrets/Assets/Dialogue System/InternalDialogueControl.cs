using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;


public class InternalDialogueControl : MonoBehaviour
{
    public GameObject thisNPC;
    public List<DialogueField> dialogueFields; // list of dialogue fields for this NPC
    public bool active = true;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (dialogueFields.Count == 0)
            {
                UnityEngine.Debug.Log("No dialogue fields assigned to this NPC.");
                return;
            }
            PlayerDialogueTrigger.interactionOccurance.AddListener(StartTalking);
        }
    }

    void StartTalking()
    {
        if (active)
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
