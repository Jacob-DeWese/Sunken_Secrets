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

    [SerializeField] protected float moveTime = 3f;
    private float timer = 0f;
    [SerializeField] protected float speed = 5f;
    [SerializeField] protected Light lightSource;

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
        if (required_npcs.Count > 0)
        {
            // TODO: Implement light source movement based on the number of required NPCs remaining
            // For example, you could adjust the intensity or range of a light source here
            lightSource.transform.Rotate(Vector3.up, (npcs.Count/270.0f) * Time.deltaTime);
        }
        if (required_npcs.Count == 0 && timer < moveTime)
        {
            Vector3 direction = new Vector3(0, -1, 0).normalized;
            ocean.transform.Translate(direction * speed * Time.deltaTime);
            timer += Time.deltaTime;
        }
        else if (required_npcs.Count == 0 && timer >= moveTime)
        {
            ocean.SetActive(false);
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
