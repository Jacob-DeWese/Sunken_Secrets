using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DialogueField", menuName = "Scriptable Objects/DialogueField")]
public class DialogueField : ScriptableObject
{
    public List<Sprite> portraits;
    public string firstSpeaker; // used to determine which portrait to show, and which name to show initially
    public string secondSpeaker; // used to determine which portrait to show, and which name to show when speaker changes
    public List<string> dialogue;

}
