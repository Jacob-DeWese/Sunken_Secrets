using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.ComponentModel.Design;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using TMPro;
using System.Reflection;

public class UI_Dialogue_Logic_Manager : MonoBehaviour
{
    private static UI_Dialogue_Logic_Manager instance;

    [Header("Dialogue logic that adds clues to a journal for each NPC encounter")]
    [Tooltip("List to store EVERY NPC who has dialogue options or interactions with the player. MUST be the gameObject, and each NPC has to have the same name across all levels")]
    [SerializeField] protected List<GameObject> npcsWithDialogue = new();

    [Tooltip("List for all the images that each NPC will have in the journal")]
    [SerializeField] protected List<Sprite> npcJournalHeadshot = new();

    [SerializeField] private Image headshotImage;

    [Tooltip("List for all the clues that the npcsWithDialogue list will have")]
    [SerializeField] protected List<TextAsset> cluesFromNPCS = new();

    [Tooltip("Ordered map/dictionary used by the journal")]
    private Dictionary<string, List<string>> journal = new Dictionary<string, List<string>>() {};

    [SerializeField] private TextMeshProUGUI journalText;
    
    [SerializeField] private TextMeshProUGUI characterNameText;

    [SerializeField] private GameObject journalParent;

    [Tooltip("Next button to move to the next page in the journal")]
    [SerializeField] protected GameObject nextButton;

    [Tooltip("Previous button to move to the previous page in the journal")]
    [SerializeField] protected GameObject prevButton;

    private bool journalParentActive;

    private int currentNpcIndex = 0;


    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
        // DontDestroyOnLoad(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < npcsWithDialogue.Count; i++)
        {
            journal.Add(npcsWithDialogue[i].name, new List<string>());
        }

        headshotImage.gameObject.SetActive(false);

        journalText.text = "";
        characterNameText.text = "";

        journalParent.SetActive(false);
        journalParentActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            journalParentActive = !journalParentActive;
            journalParent.SetActive(journalParentActive);

            if (journalParentActive)
            {
                UpdateJournalUI();
            }
        }
    }

    public void nextPage()
    {
        if (npcsWithDialogue.Count == 0)
        {
            return;
        }

        currentNpcIndex++;

        if (currentNpcIndex >= npcsWithDialogue.Count)
        {
            currentNpcIndex = 0;
        }

        UpdateJournalUI();
    }

    public void prevPage()
    {
        if (npcsWithDialogue.Count == 0)
        {
            return;
        }

        currentNpcIndex--;

        if (currentNpcIndex < 0)
        {
            currentNpcIndex = npcsWithDialogue.Count - 1;
        }

        UpdateJournalUI();
    }

    void UpdateJournalUI()
    {
        if (npcsWithDialogue.Count == 0)
        {
            return;
        }

        string currentNPC = npcsWithDialogue[currentNpcIndex].name;

        if (journal.TryGetValue(currentNPC, out List<string> clues))
        {
            if (clues.Count == 0)
            {
                journalText.text = "";
                characterNameText.text = "";

                headshotImage.gameObject.SetActive(false);
            }
            else
            {
                if (currentNpcIndex < npcJournalHeadshot.Count)
                {
                    headshotImage.sprite = npcJournalHeadshot[currentNpcIndex];
                    headshotImage.gameObject.SetActive(true);
                }
                else
                {
                    headshotImage.gameObject.SetActive(false);
                }

                journalText.text = string.Join("\n\n", clues);
                characterNameText.text = currentNPC;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("NPC_Required") || other.gameObject.CompareTag("NPC_Optional"))
        {
            string npcName = other.gameObject.name;

            if (journal.ContainsKey(npcName)) {
                string clueIndex = SceneManager.GetActiveScene().buildIndex.ToString();
                string clueTextFileName = other.gameObject.name + "_Clue_Dialogue_" + clueIndex;
                string path = Path.Combine(Application.dataPath, "Dialogue System", "All_Dialogue_Files", clueTextFileName + ".txt");

                if (File.Exists(path))
                {
                    string contents = File.ReadAllText(path);
                    Debug.Log(contents);

                    if (!journal[npcName].Contains(contents))
                    {
                        journal[npcName].Add(contents);

                        for (int i = 0; i < npcsWithDialogue.Count; i++)
                        {
                            if (npcsWithDialogue[i].name == npcName)
                            {
                                currentNpcIndex = i;
                                break;
                            }
                        }
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
