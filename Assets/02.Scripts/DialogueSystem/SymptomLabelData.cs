using System.Collections.Generic;

[System.Serializable]
public class LabelEntry
{
    public string Key;
    public string Value;
}


[System.Serializable]
public class SymptomLabelData
{
    public List<LabelEntry> Labels;
}