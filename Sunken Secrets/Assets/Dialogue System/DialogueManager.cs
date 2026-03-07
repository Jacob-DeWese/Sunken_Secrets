using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
//[RequireComponent(typeof(TMP_Text))]

public class DialogueManager : MonoBehaviour
{
    //DialogueField is the scriptable object!
    public static UnityEvent<List<DialogueField>> NPCSpeaking = new UnityEvent<List<DialogueField>>();
    public static UnityEvent TextPrinted = new UnityEvent();
    List<DialogueField> TextRef; // stored dialogue
    List<GameObject> instantiatedObjects = new List<GameObject>();
    public GameObject textParent; // parent object for text, used to enable/disable dialogue box
    public GameObject portraitParent; // parent object for portrait, used to enable/disable portrait
    public GameObject nameParent; // parent object for name, used to enable/disable name
    public GameObject choiceParent;
    public GameObject dialogueChoicePrefab;
    TMP_Text textComponent; // actual text on screen
    int dialogueOrder;
    int dialogueSet;



    // Typewriter effect:
    float charDelay = 0.05f;
    void Awake()
    {
        // Runs only once, on script instance creation. Used for initialization.
        NPCSpeaking.AddListener(DialogueCall); // run dialogeue when event is called
        textParent.SetActive(false);
    }

    void Update()
    {
        // Runs every frame. Used for input checks and other things that need to be checked constantly.
        if (Input.GetKeyDown(KeyCode.E))
            DialogueCall(TextRef);
    }

    public void DialogueCall(List<DialogueField> localText)
    {
    
        
            // activeInHierarchy checks if active IN SCENE, not just if its active
            if (!textParent.activeInHierarchy)
            {
                textParent.SetActive(true);
                textComponent = GetComponent<TMP_Text>();
                dialogueOrder = 0;
                dialogueSet = 0;
                TextRef = localText;
                textComponent.text = TextRef[dialogueSet].dialogue[dialogueOrder];
                StartCoroutine(WriteChar());
            }
            else
            {
                StopAllCoroutines(); // only dialogue should be running

                if (textComponent.maxVisibleCharacters < textComponent.text.Length)
                    textComponent.maxVisibleCharacters = textComponent.text.Length;
                
                else
                {
                    
                    dialogueOrder++;
                    // CHOICE
                    // if (dialogueOrder < TextRef[dialogueSet].dialogue.Count && gameObject.activeInHierarchy)
                    // {

                    //     if (TextRef[dialogueSet].dialogue[dialogueOrder].Split('*').Length > 2)
                    //     {
                    //         float buttonOffset = 0;
                    //         NPCSpeaking.RemoveListener(DialogueCall);
                    //         choiceParent.SetActive(true);
                    //         textComponent.text = TextRef[dialogueSet].dialogue[dialogueOrder].Split('*')[0];
                    //         for (int i = 1; i < TextRef[dialogueSet].dialogue[dialogueOrder].Split('*').Length; i++)
                    //         {
                    //             int buttonIndex = dialogueSet + i;
                    //             GameObject choiceButton = Instantiate(dialogueChoicePrefab, choiceParent.transform);
                    //             choiceButton.GetComponent<RectTransform>().position = new Vector3(choiceButton.GetComponent<RectTransform>().position.x, choiceButton.GetComponent<RectTransform>().position.y - buttonOffset, 0);
                    //             choiceButton.GetComponentInChildren<TMP_Text>().text = TextRef[dialogueSet].dialogue[dialogueOrder].Split('*')[i];
                    //             choiceButton.GetComponent<Button>().onClick.AddListener(() => ChoiceCall(buttonIndex));
                    //             instantiatedObjects.Add(choiceButton);
                    //             buttonOffset += 30;
                    //         }
                    //     }
                    //     else
                    //         textComponent.text = TextRef[dialogueSet].dialogue[dialogueOrder];
                    //     StartCoroutine(WriteChar());
                    // }
                    // END OF CHOICE

                    if (dialogueOrder >= TextRef[dialogueSet].dialogue.Count) {
                        textParent.SetActive(false);
                        return;
                    }

                    textComponent.text = TextRef[dialogueSet].dialogue[dialogueOrder];
                    StartCoroutine(WriteChar());
                    
                }

        }
    }
    public void ChoiceCall(int choice)
    {
        dialogueSet = choice;
        dialogueOrder = -1;
        foreach (GameObject choiceButton in instantiatedObjects)
            Destroy(choiceButton);
        instantiatedObjects.Clear();
        NPCSpeaking.AddListener(DialogueCall);
        choiceParent.SetActive(false);
        DialogueCall(TextRef);
    }
    public void setUI()
    {
    
    }


    IEnumerator WriteChar()
    {
        textComponent.maxVisibleCharacters = 0;
        foreach (char character in textComponent.text)
        {
            textComponent.maxVisibleCharacters++;
            yield return new WaitForSeconds(charDelay);
        }
        textComponent.maxVisibleCharacters = 99999;
        TextPrinted.Invoke();
    }
}
