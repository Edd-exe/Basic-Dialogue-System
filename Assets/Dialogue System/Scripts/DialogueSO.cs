using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueSO", menuName = "Dialogue System/DialogueData", order = 0)]
public class DialogueSO : ScriptableObject 
{
    public List<DialogueVariables> Dialogues;
}