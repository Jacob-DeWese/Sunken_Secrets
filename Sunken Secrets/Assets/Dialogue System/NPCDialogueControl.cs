using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;


public class NPCDialogueControl : MonoBehaviour
{
    public List<DialogueField> dialogueFields; // list of dialogue fields for this NPC
    public List<DialogueField> repeatedFields; // dialogue fields for repeated interaction
    // bool to control repeated interactions
    public bool repeat = false;
    
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
        if (!repeat)
            DialogueManager.NPCSpeaking.Invoke(dialogueFields);
        else
            DialogueManager.NPCSpeaking.Invoke(repeatedFields);
            
        
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerDialogueTrigger.interactionOccurance.RemoveListener(StartTalking);
        }
    }
}
