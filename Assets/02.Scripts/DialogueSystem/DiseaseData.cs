using UnityEngine;

[CreateAssetMenu(fileName = "NewDisease", menuName = "Disease/Create New Disease")]
public class DiseaseData : ScriptableObject
{
    public string DiseaseName;
    public Symptom Symptoms;

    [TextArea]
    public string Description;
}