using System;
using System.Collections.Generic;

[Serializable]
public class DialogueChoice
{
    public string Text;
    public string Next;
    public string RequiredFlag;
}

[Serializable]
public class DialogueLine
{
    public string Id;
    public string Speaker;
    public string Text;
    public string Next;

    public bool IsChoice;
    public List<DialogueChoice> Choices;
}