using UnityEngine;

// 질병 데이터를 담는 ScriptableObject 클래스
[CreateAssetMenu(fileName = "NewDisease", menuName = "Disease/Create New Disease")]
public class DiseaseData : ScriptableObject
{
    public string DiseaseName;           // 질병 이름
    public Symptom Symptoms;             // 질병에 해당하는 증상들 (EnumFlags 사용 가능)

    [TextArea]
    public string Description;           // 질병 설명 텍스트
}