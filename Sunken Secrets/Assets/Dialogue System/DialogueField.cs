using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DialogueField", menuName = "Scriptable Objects/DialogueField")]
public class DialogueField : ScriptableObject
{
    public List<Sprite> portraits;
    public string NPCName;
    public List<string> dialogue;
}
