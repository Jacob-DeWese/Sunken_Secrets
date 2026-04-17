using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;


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

public class UI_Notepad_Manager : MonoBehaviour
{
    private static UI_Notepad_Manager instance;

    [Header("NPC Order List")]
    [Tooltip("List to store EVERY NPC who has dialogue options or interactions with the player. MUST be the gameObject, and each NPC has to have the same name across all levels")]
    [SerializeField] protected List<GameObject> npcList = new();
    [Tooltip("List for all the orders that the npcs will have")]
    [SerializeField] protected List<TextAsset> npcOrders = new();

    [Header("Notepad UI")]
    [Tooltip("Ordered map/dictionary used by the journal")]
    [SerializeField] private TextMeshProUGUI characterNameText;
    [SerializeField] private TextMeshProUGUI characterOrderText;
    [SerializeField] private GameObject notepadParent;

    private Dictionary<string, List<string>> notepad = new();
    private Dictionary<string, TextAsset> npcOrderLookup = new();
    private bool notepadParentActive;
    private int currentNpcIndex = 0;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;

        notepadParent.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        npcList.RemoveAll(npc => npc == null);

        for (int i = 0; i < npcList.Count; i++)
        {
            if (npcList[i] != null)
            {
                string npcName = npcList[i].name;

                if (!notepad.ContainsKey(npcName))
                {
                    notepad[npcName] = new List<string>();
                }
                if (i < npcOrders.Count && npcOrders[i] != null)
                {
                    npcOrderLookup[npcName] = npcOrders[i];
                }
            }
        }

        characterNameText.text = "";
        characterNameText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            bool newState = !notepadParent.activeSelf;
            notepadParent.SetActive(newState);

            if (newState)
            {
                UpdateNotepadUI();
            }
        }
    }

    void UpdateNotepadUI()
    {
        if (npcList.Count == 0)
        {
            return;
        }

        if (currentNpcIndex >= npcList.Count || npcList[currentNpcIndex] == null)
        {
            currentNpcIndex = 0;
            return;
        }

        string currentNPC = npcList[currentNpcIndex].name;

        characterNameText.text = currentNPC;

        if (notepad.TryGetValue(currentNPC, out List<string> orders))
        {
            if (orders.Count == 0)
            {
                characterOrderText.text = "";
                return;
            }
            
            characterOrderText.text = string.Join("\n\n", orders);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("NPC_Required") || !other.gameObject.CompareTag("NPC_Optional"))
        {
            return;
        }

        string npcName = other.gameObject.name;

        if (!npcOrderLookup.ContainsKey(npcName))
        {
            return;
        }

        pendingNpc = npcName;
    }

    public void NPCDialogue()
    {
        if (string.IsNullOrEmpty(pendingNpc))
        {
            return;
        }
        if (!npcOrderLookup.ContainsKey(pendingNpc))
        {
            return;
        }

        TextAsset characterOrderFile = npcOrderLookup[pendingNpc];

        if (orderFile == null)
        {
            return;
        }

        string contents = orderFile.text;

        if (!notepad[pendingNpc].Contains(contents))
        {
            notepad[pendingNpc].Add(contents);
        }

        currentNpcIndex = npcList.FindIndex(n => n != null && n.name == pendingNpc);

        UpdateNotepadUI();

        pendingNpc = null;
    }
}
