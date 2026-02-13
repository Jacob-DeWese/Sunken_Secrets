using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.ComponentModel.Design;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using TMPro;

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

    * There needs to be controls for the journal that also allows you to move pages, but that will be separate functionality within here
    */

    [Header("Dialogue logic that adds clues to a journal for each NPC encounter")]
    [Tooltip("List to store EVERY NPC who has dialogue options or interactions with the player. MUST be the gameObject, and each NPC has to have the same name across all levels")]
    [SerializeField] protected List<GameObject> npcsWithDialogue = new();

    [Tooltip("List for all the images that each NPC will have in the journal")]
    [SerializeField] protected List<Image> npcJournalHeadshot = new();

    [Tooltip("List for all the clues that the npcsWithDialogue list will have")]
    [SerializeField] protected List<TextAsset> cluesFromNPCS = new();

    [Tooltip("Ordered map/dictionary used by the journal")]
    private Dictionary<GameObject, List<string>> journal = new Dictionary<GameObject, List<string>>() {};

    [SerializeField] private TextMeshProUGUI journalText;

    private int currentNpcIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < npcsWithDialogue.Count; i++)
        {
            journal.Add(npcsWithDialogue[i], new List<string>());

            if (i < npcJournalHeadshot.Count)
            {
                npcJournalHeadshot[i].gameObject.SetActive(false);
            }
        }

        journalText.text = "";
    }

    // Update is called once per frame
    void Update()
    {

    }

    void UpdateJournalUI()
    {
        if (npcsWithDialogue.Count == 0)
        {
            return;
        }

        GameObject currentNPC = npcsWithDialogue[currentNpcIndex];

        if (journal.TryGetValue(currentNPC, out List<string> clues))
        {
            if (clues.Count == 0)
            {
                journalText.text = "";

                if (currentNpcIndex < npcJournalHeadshot.Count)
                {
                    npcJournalHeadshot[currentNpcIndex].gameObject.SetActive(false);
                }
            }
            else
            {
                if (currentNpcIndex < npcJournalHeadshot.Count)
                {
                    npcJournalHeadshot[currentNpcIndex].gameObject.SetActive(true);
                }

                journalText.text = string.Join("\n\n", clues);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("NPC_Required") || other.gameObject.CompareTag("NPC_Optional"))
        {
            if (journal.ContainsKey(other.gameObject)) {
                string clueIndex = SceneManager.GetActiveScene().buildIndex.ToString();
                string clueTextFileName = other.gameObject.name + "_Clue_Dialogue_" + clueIndex;
                string path = Path.Combine(Application.dataPath, "Dialogue System", "All_Dialogue_Files", clueTextFileName + ".txt");

                if (File.Exists(path))
                {
                    string contents = File.ReadAllText(path);
                    Debug.Log(contents);

                    if (!journal[other.gameObject].Contains(contents))
                    {
                        journal[other.gameObject].Add(contents);

                        currentNpcIndex = npcsWithDialogue.IndexOf(other.gameObject);
                        UpdateJournalUI();
                    }
                }
                else
                {
                    Debug.LogError("File not found: " + path);
                }
            }
        }
    }
}
