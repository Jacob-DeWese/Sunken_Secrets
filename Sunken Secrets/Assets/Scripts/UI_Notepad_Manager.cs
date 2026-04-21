using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;

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
    public static UI_Notepad_Manager Instance => instance;

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
                characterNameText[i].text = $"Table {i + 1}";
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
            
            characterNameText[slotIndex].text = $"Table {slotIndex + 1}";
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

    public void MarkOrderCompleted(string npcName, string orderText)
    {
        if (!notepad.ContainsKey(npcName))
            return;

        var list = notepad[npcName];

        for (int i = 0; i < list.Count; i++)
        {
            // match raw text OR already formatted text
            if (list[i].Contains(orderText))
            {
                list[i] = $"<s>{orderText}</s>";
                break;
            }
        }

        UpdateNotepadUI();
    }
}
