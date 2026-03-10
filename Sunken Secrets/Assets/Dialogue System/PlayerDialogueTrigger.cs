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
            // safe guard to ensure conversations don't happen repeatedly
            else if (!spoken_to_npcs.Contains(other.gameObject))
            {
                dialoguePrefab.SetActive(true);
                interactionOccurance.Invoke();
                UnityEngine.Debug.Log("Interaction Occurred");
                
                
            }
            else
                UnityEngine.Debug.Log("NPC already interacted with");
            
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
                spoken_to_npcs.Add(other.gameObject);
            }
        }
    }
}
