using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class PlayerDialogueTrigger : MonoBehaviour
{
    // this script stores information needed to trigger a dialogue interaction with an NPC


    //public --> any script can access
    //static --> only one instance of this variable can exist, and it is shared across all instances of the class
    public static UnityEvent interactionOccurance = new UnityEvent();
    public List<GameObject> spoken_to_npcs = new List<GameObject>();

    public GameObject dialoguePrefab;

    void Start()
    {
        dialoguePrefab.SetActive(false);
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("NPC_Required") || other.gameObject.CompareTag("NPC_Optional"))
        {
            if (other.gameObject.GetComponent<NPCDialogueControl>().dialogueFields.Count == 0)
            {
                UnityEngine.Debug.Log("No dialogue fields assigned to this NPC.");
                return;
            }
        
            else
            {
                dialoguePrefab.SetActive(true);
                interactionOccurance.Invoke();  
                UnityEngine.Debug.Log("Interaction Occurred");
            }
            
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("NPC_Required") || other.gameObject.CompareTag("NPC_Optional"))
        {
            dialoguePrefab.SetActive(false);    
            interactionOccurance.RemoveAllListeners();
            UnityEngine.Debug.Log("Interaction Ended");
            if (!spoken_to_npcs.Contains(other.gameObject))
            {
                // if this is a new NPC interaction, add to spoken to list, and set repeat bool to true
                other.gameObject.GetComponent<NPCDialogueControl>().repeat = true;
                spoken_to_npcs.Add(other.gameObject);
                
               
                
            }
        }
    }
}