using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class NPCDialogueControl : MonoBehaviour {

    public List<DialogueField> dialogueFields; // list of dialogue fields for this NPC / internal dialogue
    public List<DialogueField> repeatedFields; // dialogue fields for repeated interaction
    
    // REPEAT CONTROL
    // bool to control repeated NPC interactions
    public bool repeat = false;
    // bool to disable revisiting internal dialogue
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
        if (this.gameObject.CompareTag("NPC_Required") || this.gameObject.CompareTag("NPC_Optional")) {
            if (!repeat)
                DialogueManager.NPCSpeaking.Invoke(dialogueFields);
            else if (repeat)
                DialogueManager.NPCSpeaking.Invoke(repeatedFields);
        }
        else if (this.gameObject.CompareTag("Internal"))
        {
            if (active)
                DialogueManager.NPCSpeaking.Invoke(dialogueFields);
        }
            
        
    }

    void OnTriggerExit(Collider other)
    { 
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerDialogueTrigger.interactionOccurance.RemoveListener(StartTalking);
        }
    }
}
