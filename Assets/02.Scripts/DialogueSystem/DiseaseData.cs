using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "NewDisease", menuName = "Disease/Create New Disease")]
public class DiseaseData : ScriptableObject
{
    public string DiseaseName;
    public Symptom Symptoms;
    public Sprite MedicineIcon;

    [TextArea]
    public string Description;
}