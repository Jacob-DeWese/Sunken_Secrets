// Unity Starter Package - Version 1
// University of Florida's Digital Worlds Institute
// Written by Michael O'Connell, then edited by Benjamin Cohen, Eric Bejleri, and Logan Kemper

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

namespace DigitalWorlds.Dialogue
{
    /// <summary>
    /// DialogueManager handles the dialogue that is sent by the DialogueTrigger. The sent text is navigated by DialogueManager and then displayed on the UI.
    /// </summary>
    public class DialogueManager : MonoBehaviour
    {
        [System.Serializable]
        public class DialogueEvents
        {
            [Space(10)]
            public UnityEvent onDialogueBegan, onDialogueEnded;
        }

        [Header("Speaker Library")]
        [Tooltip("SpeakerLibrary ScriptableObject goes in here to access your list of speakers and their sprites.")]
        [SerializeField] private SpeakerLibrary speakerLibrary;

        [Header("UI Elements")]
        [Tooltip("The parent GameObject of all the dialogue UI elements.")]
        [SerializeField] private GameObject dialogueParent;

        [Tooltip("The body text element.")]
        [SerializeField] private TMP_Text textBox;

        [Tooltip("The nameplate text element.")]
        [SerializeField] private TMP_Text nameText;

        [Tooltip("Image where the speaker sprites will appear.")]
        [SerializeField] private Image speakerImage;

        [Tooltip("This is the indicator image/button that shows that the next text can be shown.")]
        [SerializeField] private GameObject continueImage;

        [Header("Options")]
        [Tooltip("Controls whether the texts scrolls or appears instantly.")]
        [SerializeField] private bool scrollText = true;

        [Tooltip("The speed of the scrolling text (in seconds per character).")]
        [SerializeField] private float typeSpeed = 0.01f;

        [Header("Events")]
        [SerializeField] private DialogueEvents dialogueEvents;

        public DialogueTrigger CurrentTrigger { get; set; }
        public bool IsInDialogue { get; private set; } = false;

        private readonly Dictionary<string, SpeakerLibrary.SpeakerInfo> speakerDictionary = new();
        private Queue<string> inputStream = new();
        private Coroutine scrollCoroutine;
        private bool isTyping = false;
        private bool cancelTyping = false;

        private void Start()
        {
            dialogueParent.SetActive(false);

            foreach (var info in speakerLibrary.speakerList)
            {
                speakerDictionary[info.name] = info;
            }
        }

        public void StartDialogue(Queue<string> dialogue)
        {
            IsInDialogue = true;
            dialogueParent.SetActive(true);
            continueImage.SetActive(false);
            inputStream = dialogue;
            AdvanceDialogue();
            dialogueEvents.onDialogueBegan.Invoke();
        }

        public void AdvanceDialogue()
        {
            if (!IsInDialogue)
            {
                return;
            }

            if (isTyping)
            {
                cancelTyping = true;
                return;
            }

            if (inputStream.Peek().Contains("EndQueue")) // Phrase to stop dialogue
            {
                inputStream.Dequeue();
                EndDialogue();
            }
            else if (inputStream.Peek().Contains("[NAME=")) // Set the name of the speaker
            {
                string name = inputStream.Peek();
                name = inputStream.Dequeue().Substring(name.IndexOf('=') + 1, name.IndexOf(']') - (name.IndexOf('=') + 1));

                if (speakerDictionary.TryGetValue(name, out SpeakerLibrary.SpeakerInfo speaker))
                {
                    nameText.text = name;
                    nameText.color = speaker.nameColor;
                    textBox.color = speaker.textColor;
                }
                else
                {
                    Debug.LogWarning("Name not found in Speaker Library");
                }

                AdvanceDialogue();
            }
            else if (inputStream.Peek().Contains("[SPEAKERSPRITE=")) // Set the speaker sprite (if present)
            {
                string part = inputStream.Peek();
                string spriteName = inputStream.Dequeue().Substring(part.IndexOf('=') + 1, part.IndexOf(']') - (part.IndexOf('=') + 1));

                if (speakerDictionary.TryGetValue(spriteName, out SpeakerLibrary.SpeakerInfo speaker))
                {
                    speakerImage.sprite = speaker.sprite;
                }

                AdvanceDialogue();
            }
            else
            {
                if (scrollText)
                {
                    if (!isTyping)
                    {
                        string textString = inputStream.Dequeue();

                        if (scrollCoroutine != null)
                        {
                            StopCoroutine(scrollCoroutine);
                        }
                        scrollCoroutine = StartCoroutine(TextScroll(textString));
                    }
                    else if (isTyping && !cancelTyping)
                    {
                        cancelTyping = true;
                    }
                }
                else
                {
                    textBox.text = inputStream.Dequeue();
                    continueImage.SetActive(true);
                }
            }

            speakerImage.gameObject.SetActive(speakerImage.sprite != null);
        }

        private IEnumerator TextScroll(string lineOfText)
        {
            continueImage.SetActive(false);
            int letter = 0;
            textBox.text = "";
            isTyping = true;
            cancelTyping = false;

            while (isTyping && !cancelTyping && (letter < lineOfText.Length - 1))
            {
                textBox.text += lineOfText[letter];
                letter++;
                yield return new WaitForSeconds(typeSpeed);
            }

            textBox.text = lineOfText;
            continueImage.SetActive(true);
            isTyping = false;
            cancelTyping = false;
        }

        [ContextMenu("End Dialogue")]
        public void EndDialogue()
        {
            if (!IsInDialogue)
            {
                return;
            }

            textBox.text = "";
            nameText.text = "";
            inputStream.Clear();
            dialogueParent.SetActive(false);

            IsInDialogue = false;
            cancelTyping = false;
            isTyping = false;

            if (CurrentTrigger.singleUse)
            {
                CurrentTrigger.hasBeenUsed = true;
            }

            inputStream.Clear();

            dialogueEvents.onDialogueEnded.Invoke();
            CurrentTrigger.DialogueEnded();
        }

        private void OnValidate()
        {
            // Clamp typeSpeed to 0 in the inspector
            typeSpeed = Mathf.Max(0f, typeSpeed);
        }
    }
}