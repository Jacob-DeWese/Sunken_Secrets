// Unity Starter Package - Version 1
// University of Florida's Digital Worlds Institute
// Written by Michael O'Connell, then edited by Benjamin Cohen, Eric Bejleri, and Logan Kemper

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DigitalWorlds.Dialogue
{
    /// <summary>
    /// Sends dialogue to the DialogueManager via trigger collision or button press.
    /// </summary>
    public class DialogueTrigger : MonoBehaviour
    {
        [System.Serializable]
        public class DialogueTriggerEvents
        {
            [Space(10)]
            public UnityEvent onDialogueBegan, onDialogueEnded;
        }

        public enum TriggerType : byte
        {
            TriggerCollision,
            KeyPress,
            EventOnly
        }

        [Tooltip("The button input used for advancing/starting dialogue. Set to the E key by default.")]
        [SerializeField] private KeyCode buttonInput = KeyCode.E;

        [Tooltip("Drag in the DialogueManager.")]
        [SerializeField] private DialogueManager dialogueManager;

        [Tooltip("Drag in the text file for this dialogue.")]
        [SerializeField] private TextAsset textFile;

        [Tooltip("Enter the tag name that should register collisions.")]
        [SerializeField] private string tagName = "Player";

        [Tooltip("How long in seconds before a new line of dialogue can be skipped.")]
        [SerializeField] private float waitTime = 0.5f;

        [Tooltip("If true, this dialogue can only be triggered one time.")]
        public bool singleUse = false;

        [Tooltip("Choose whether dialogue will be triggered by a key press, a trigger collision, or only from UnityEvents.")]
        [SerializeField] private TriggerType triggerType;

        [HideInInspector] public bool hasBeenUsed = false;

        [SerializeField] private DialogueTriggerEvents triggerEvents;

        private readonly Queue<string> dialogue = new();
        private bool inArea = false;
        private float nextTime = 0f;

        public void SetWaitTime(float waitTime)
        {
            this.waitTime = waitTime;
        }

        private void Start()
        {
            if (dialogueManager == null)
            {
                dialogueManager = FindAnyObjectByType<DialogueManager>();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(buttonInput) && !hasBeenUsed)
            {
                if (dialogueManager.IsInDialogue && dialogueManager.CurrentTrigger != this)
                {
                    return;
                }

                bool canReceiveInput = (triggerType == TriggerType.KeyPress && inArea) || dialogueManager.IsInDialogue;
                if (!canReceiveInput)
                {
                    return;
                }

                if (!dialogueManager.IsInDialogue)
                {
                    if (triggerType == TriggerType.KeyPress && inArea)
                    {
                        TriggerDialogue();
                    }
                }
                else if (nextTime < Time.timeSinceLevelLoad)
                {
                    nextTime = Time.timeSinceLevelLoad + waitTime;
                    dialogueManager.AdvanceDialogue();
                }
            }
        }

        [ContextMenu("Trigger Dialogue")]
        public void TriggerDialogue()
        {
            dialogueManager.CurrentTrigger = this;
            ReadTextFile();
            dialogueManager.StartDialogue(dialogue);
            triggerEvents.onDialogueBegan.Invoke();
        }

        public void DialogueEnded()
        {
            triggerEvents.onDialogueEnded.Invoke();
        }

        private void ReadTextFile()
        {
            dialogue.Clear();
            string txt = textFile.text;
            string[] lines = txt.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                int tagEnd;
                string currentLine = line;

                // Process any tags at the beginning of the line
                while (currentLine.StartsWith("[") && (tagEnd = currentLine.IndexOf(']')) != -1)
                {
                    string tag = currentLine.Substring(0, tagEnd + 1);
                    dialogue.Enqueue(tag);
                    currentLine = currentLine.Substring(tagEnd + 1);
                }

                // Add the remaining text if any
                if (!string.IsNullOrEmpty(currentLine))
                {
                    dialogue.Enqueue(currentLine);
                }
            }

            dialogue.Enqueue("EndQueue");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (string.IsNullOrEmpty(tagName) || (other.CompareTag(tagName) && !hasBeenUsed))
            {
                if (triggerType == TriggerType.TriggerCollision)
                {
                    TriggerDialogue();
                }

                inArea = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (string.IsNullOrEmpty(tagName) || other.CompareTag(tagName))
            {
                if (triggerType != TriggerType.EventOnly)
                {
                    dialogueManager.EndDialogue();
                }

                inArea = false;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (string.IsNullOrEmpty(tagName) || (collision.CompareTag(tagName) && !hasBeenUsed))
            {
                if (triggerType == TriggerType.TriggerCollision)
                {
                    TriggerDialogue();
                }

                inArea = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (string.IsNullOrEmpty(tagName) || collision.CompareTag(tagName))
            {
                if (triggerType != TriggerType.EventOnly)
                {
                    dialogueManager.EndDialogue();
                }

                inArea = false;
            }
        }

        private void OnValidate()
        {
            // Clamp waitTime to 0 in the inspector
            waitTime = Mathf.Max(0, waitTime);
        }
    }
}