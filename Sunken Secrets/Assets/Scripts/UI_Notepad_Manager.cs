// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;
// using Unity.VisualScripting;
// using System.ComponentModel.Design;
// using UnityEngine.SceneManagement;
// using System.IO;
// using UnityEngine.UI;
// using TMPro;

// /*
// How I am thinking about how the notepad/order taking mechanics will work:
// - Each NPC will have a marker for what their order is
//     - In whatever script is tracking the order (either UI_Notepad_Manager or NPC_Tracking), make a public variable that takes in a GameObject of the food for that NPC
// - When you interact with an NPC, behind the scenes the next available name in the list is updated to the NPC's name
// - Each NPC's dialogue must have their order at the end (not for tracking purposes, but just for the sake of logical progression)
// - Only once the dialogue interaction ends will you see the order appear in the notepad
//     - Check what the GameObject is associated with what the NPC ordered (e.g. 'Cheeseburger') and find the associated order txt document (e.g. 'Cheeseburger Order') 
//     - The GameObject can be the key for a map, and the text document can be the value
// - A separate list will store the order in which the GameObjects have been assigned (it will have a set size)
// - When the list is full, randomly pick which order to fulfill first (every order must be unique)
// - When the food is taken back to the right person, remove it from the list and continue until list is empty
// */

// public class UI_Notepad_Manager : MonoBehaviour
// {
//     private static UI_Notepad_Manager instance;

//     [Header("Dialogue logic that adds clues to a journal for each NPC encounter")]
//     [Tooltip("List to store EVERY NPC who has dialogue options or interactions with the player. MUST be the gameObject, and each NPC has to have the same name across all levels")]
//     [SerializeField] protected List<GameObject> npcsWithDialogue = new();

//     [Tooltip("List for all the orders that the npcs will have")]
//     [SerializeField] protected List<TextAsset> npcOrders = new();

//     [Tooltip("Ordered map/dictionary used by the journal")]
//     private Dictionary<string, List<string>> journal = new Dictionary<string, List<string>>() {};

//     [SerializeField] private TextMeshProUGUI journalText;
    
//     [SerializeField] private TextMeshProUGUI characterNameText;

//     [SerializeField] private GameObject notepadParent;


//     private bool notepadParentActive;

//     private int currentNpcIndex = 0;


//     void Awake()
//     {
//         if (instance != null && instance != this)
//         {
//             Destroy(this);
//             return;
//         }

//         instance = this;

//         notepadParent.SetActive(false);
//     }

//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     void Start()
//     {
//         npcsWithDialogue.RemoveAll(npc => npc == null);

//         for (int i = 0; i < npcsWithDialogue.Count; i++)
//         {
//             journal.Add(npcsWithDialogue[i].name, new List<string>());
//         }

//         headshotImage.gameObject.SetActive(false);

//         journalText.text = "";
//         characterNameText.text = "";

//     }

//     // Update is called once per frame
//     void Update()
//     {
//         if (Input.GetKeyDown(KeyCode.J))
//         {
//             bool newState = !notepadParent.activeSelf;
//             notepadParent.SetActive(newState);

//             if (newState)
//             {
//                 UpdateJournalUI();
//             }
//         }
//     }

//     void UpdateJournalUI()
//     {
//         if (npcsWithDialogue.Count == 0)
//         {
//             return;
//         }

//         if (currentNpcIndex >= npcsWithDialogue.Count || npcsWithDialogue[currentNpcIndex] == null)
//         {
//             currentNpcIndex = 0;
//             return;
//         }

//         string currentNPC = npcsWithDialogue[currentNpcIndex].name;

//         characterNameText.text = currentNPC;

//         if (journal.TryGetValue(currentNPC, out List<string> clues))
//         {
//             if (clues.Count == 0)
//             {
//                 journalText.text = "";
//                 characterNameText.text = "";

//                 headshotImage.gameObject.SetActive(false);
//             }
//             else
//             {
//                 if (currentNpcIndex < npcJournalHeadshot.Count)
//                 {
//                     headshotImage.sprite = npcJournalHeadshot[currentNpcIndex];
//                     headshotImage.gameObject.SetActive(true);
//                 }
//                 else
//                 {
//                     headshotImage.gameObject.SetActive(false);
//                 }

//                 journalText.text = string.Join("\n\n", clues);
//                 characterNameText.text = currentNPC;
//             }
//         }
//     }

//     void OnTriggerEnter(Collider other)
//     {
//         if (other.gameObject.CompareTag("NPC_Required") || other.gameObject.CompareTag("NPC_Optional"))
//         {
//             string npcName = other.gameObject.name;

//             if (journal.ContainsKey(npcName)) {
//                 string clueIndex = SceneManager.GetActiveScene().buildIndex.ToString();
//                 string clueTextFileName = other.gameObject.name + "_Clue_Dialogue_" + clueIndex;
//                 string path = Path.Combine(Application.dataPath, "Dialogue System", "All_Dialogue_Files", clueTextFileName + ".txt");

//                 if (File.Exists(path))
//                 {
//                     string contents = File.ReadAllText(path);
//                     Debug.Log(contents);

//                     if (!journal[npcName].Contains(contents))
//                     {
//                         journal[npcName].Add(contents);

//                         for (int i = 0; i < npcsWithDialogue.Count; i++)
//                         {
//                             if (npcsWithDialogue[i] != null && npcsWithDialogue[i].name == npcName)
//                             {
//                                 currentNpcIndex = i;
//                                 break;
//                             }
//                         }
//                         UpdateJournalUI();
//                     }
//                 }
//                 else
//                 {
//                     Debug.LogError("File not found: " + path);
//                 }
//             }
//         }
//     }
// }
