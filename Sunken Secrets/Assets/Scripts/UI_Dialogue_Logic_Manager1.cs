using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class UI_Dialogue_Logic_Manager : MonoBehaviour
{
    /*
    Thinking this through: 
    *The map/Dictionary is always active, so it should be able to pull from the active GameObjects/NPCs
    - 1. There is a list of NPCs that are used throughout the game, with the names stored manually
    - 2. There is a list of all clues (manually created) that are stored in a text file named similar to the primaryDialogue file, with a shortened version of the clue stored on it
    - 3. There is an ordered map/Dictionary (new Dictionary <KeyType>, <KeyValue>)
        - 3a. KeyType will be the name of the NPC
        - 3b. The KeyValue will be a list of all the primaryDialogue (clues) that you have discovered
    * The journal will be designed with the number of NPCs with available dialogue, just those who have an empty list won't have a profile image
    - 4. When you collide with an NPC, check what the name is in the list. If the name is not already in the map, add it.
    - 5. Whether or not the NPC was already added, find what the primaryDialogue text file is in the current level
        - 5a. Find the similarly named file in the second list (e.g. the primaryDialogue is called "Will_ClueDialogue_1" and the associated text file in the second list is "Will_Clue_1")
        - 5b. To find that similary named file in list two, perform a method similar to this:
            string input = "test_cluedialogue_1";
            string target = input.Replace("dialogue", ""); // => "test_clue_1"
            Debug.Log(target); // outputs "test_clue_1"

            OR

            string input = "test_cluedialogue_1";
            string[] parts = input.Split('_'); // ["test", "cluedialogue", "1"]

            // Remove "dialogue" from the middle part
            string corrected = parts[0] + "_" + parts[1].Replace("dialogue", "") + "_" + parts[2];

            Debug.Log(corrected); // outputs "test_clue_1"
    - 6. After finding the proper clue file, take the text from the file and store it into the next index in the List KeyValue for the NPC in the map
    - 7. The clue from the text file will be added
        - 7a. When the list for that NPC is not for the first time (not empty), a profile image will be in the top corner, their name will be filled in, and the first clue will be added underneath)
        - 7b. If the list is not already empty, take the clue, put a newline in the TextMesh box, and add the next clue

    * There needs to be controls for the journal that also allows you to move pages, but that will be separate functionality
    */

    [Header("Dialogue logic for each NPC that is controlled by the player")]
    [Tooltip("List to store all the NPC Story-Critical Dialogue")]
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
