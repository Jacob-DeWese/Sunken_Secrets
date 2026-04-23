using NUnit.Framework;
using UnityEngine;
using static UnityEngine.ParticleSystem;
using System.Collections;
using System.Collections.Generic;

public class Particle : MonoBehaviour
{ 
    // dialogue dependent particles are particles that will only be activated after a certain dialogue has been completed.
    // This is to ensure that the player cannot skip certain parts of the game by activating particles out of order.
    
    public string particleName;

    [Tooltip("If true, colliding with this particle activates the postcedingParticle")]
    public bool trigger = false;

    [Tooltip("The next particle to activate when this one is collected")]
    public GameObject postcedingParticle;

    public bool dialogueDependent = false;
    public List<string> neededNPCs = new List<string>();
   

    void Start()
    {
        if (CompareTag("Conditional"))
        {
            gameObject.SetActive(false);
        }
        if (CompareTag("Directional"))
        {
            gameObject.SetActive(true);
        }
    }

    void OnEnable()
    {
        if (dialogueDependent)
        {
            // Add listener to dialogue manager for required dialogue completion
            NPCDialogueControl.dialogueComplete.AddListener(dialogueTrigger);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (trigger && postcedingParticle != null)
            {
                postcedingParticle.SetActive(true);
            }

            gameObject.SetActive(false);
        }
    }

    void dialogueTrigger(string npcName)
    {
        // store the npc name
        if (neededNPCs.Contains(npcName))
        {
            neededNPCs.Remove(npcName);
        }
        // when size is correct trigger the remaining particle
        if (neededNPCs.Count == 0 && postcedingParticle != null)
        {
            postcedingParticle.SetActive(true);
        }
    }
}
