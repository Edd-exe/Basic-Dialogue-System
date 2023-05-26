using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueSettings", menuName = "Dialogue System/DialogueSettings", order = 0)]
public class DialogueSettingsSO : ScriptableObject
{
    public bool ShowTalkerName;
    public float DefaultStandbyTimeRate;
    public float WaitTimeAfterAnswer;

    public List<TalkerVariables> Talkers;
    public List<DialogueData> DialogueData;
}
