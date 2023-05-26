using System;
using System.Collections.Generic;

#region ClassHolders
[Serializable]
public class DialogueData
{
    public string Name;
    public DialogueTypes Type;
    public DialogueSO Dialogue;
}

[Serializable]
public class DialogueVariables
{
    public string Name;
    public bool Loop;
    public List<SentenceVariables> Sentences;
}

[Serializable]
public class SentenceVariables
{
    public TalkerTypes Talker;
    public SentenceTypes Type;
    public string Text;
    public float StandbyTime;
    public float StandbyTimeAfterSentence;
}

[Serializable]
public class TalkerVariables
{
    public string Name;
    public TalkerTypes Type;
}
#endregion

#region EnumHolders
public enum SentenceTypes
{
    Normal,
    Question,
    Answer,
}

public enum DialogueTypes
{
    MainStory,
    Extra,
    //...
}

public enum TalkerTypes
{
    Father,
    Child,
    Heisenberg,
    //...
}
#endregion