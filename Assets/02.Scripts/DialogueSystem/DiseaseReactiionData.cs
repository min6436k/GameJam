using System.Collections.Generic;
using System;

[Serializable]
public class DiseaseReactionEntry
{
    public string Symptom;
    public List<string> Responses;
}


[Serializable]
public class DiseaseReactionData
{
    public List<DiseaseReactionEntry> Reactions;
}