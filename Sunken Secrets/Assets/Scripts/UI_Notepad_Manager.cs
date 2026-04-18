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

    [Header("Notepad UI")]
    [Tooltip("Ordered map/dictionary used by the journal")]
    [SerializeField] private List<TextMeshProUGUI> characterNameText;
    [SerializeField] private List<TextMeshProUGUI> characterOrderText;
    [SerializeField] private GameObject notepadParent;

    private Dictionary<string, List<string>> notepad = new();
    private bool notepadParentActive;

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
        for (int i = 0; i < characterNameText.Count; i++)
        {
            characterNameText[i].text = "";

        }
        for (int i = 0; i < characterNameText.Count; i++)
        {
            characterOrderText[i].text = "";

        }    
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

    int WriteNextOrder(string npcName, string orderText)
    {
        for (int i = 0; i < characterNameText.Count; i++)
        {
            if (string.IsNullOrEmpty(characterOrderText[i].text))
            {
                characterNameText[i].text = $"Table {i + 1} - {npcName}";
                characterOrderText[i].text = orderText;
                return i;
            }
        }
        return -1;
    }

    void UpdateNotepadUI()
    {
        for (int i = 0; i < characterNameText.Count; i++)
        {
            characterNameText[i].text = "";
            characterOrderText[i].text = "";
        }

        int slotIndex = 0;
        foreach (var order in notepad)
        {
            if (order.Value.Count == 0)
            {
                continue;
            }
            if (slotIndex >= characterNameText.Count)
            {
                break;
            }
            
            characterNameText[slotIndex].text = $"Table {slotIndex + 1} - {order.Key}";
            characterOrderText[slotIndex].text = string.Join("\n", order.Value);
            slotIndex++;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("NPC_Required") && !other.CompareTag("NPC_Optional"))
        {
            return;
        }

        NPC_Food_Order npcOrder = other.GetComponent<NPC_Food_Order>();

        if (npcOrder == null)
        {
            return;
        }

        string npcName = other.gameObject.name;
        string orderText = npcOrder.GetOrderText();

        if (string.IsNullOrEmpty(orderText))
        {
            return;
        }
        if (!notepad.ContainsKey(npcName))
        {
            notepad[npcName] = new List<string>();
        }
        if (!notepad[npcName].Contains(orderText))
        {
            notepad[npcName].Add(orderText);
            WriteNextOrder(npcName, orderText);
        }
    }
}
