using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class NPC_Tracking : MonoBehaviour
{
    /*
    Thinking this through: 
    - Make a list of all NPCs in each level, either tagged as required or not
    - For each GameObject in the list, if they are required, push that GameObject to another list called "required list"
    - When collided with, remove them from the required list
    - When the required list is empty, move the ocean to reveal the sunken city
    - *Encouraged: make a range for the light source to move to emulate daylight, take size of required list and move the light range/size of list degrees
    */

    [Header("Ocean Trigger System")]
    [Tooltip("List to store all the NPC GameObjects")]
    [SerializeField] protected List<GameObject> npcs = new();

    [Tooltip("List for all the required NPCs")]
    [SerializeField] protected List<GameObject> required_npcs = new();

    [Tooltip("Ocean GameObject")]
    [SerializeField] protected GameObject ocean;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < npcs.Count; i++)
        {
            if (npcs[i].CompareTag("NPC_Required"))
            {
                required_npcs.Add(npcs[i]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (required_npcs.Count == 0 && ocean.transform.position.z >= -30)
        {
            ocean.transform.Translate(0, 0, Time.deltaTime * -5);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("NPC_Required"))
        {
            required_npcs.Remove(other.gameObject);
        }
    }
}
