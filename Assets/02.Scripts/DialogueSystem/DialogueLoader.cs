using UnityEngine;

// 대사 및 리액션 데이터 JSON 파일을 불러오는 클래스
public class DialogueLoader
{
    // 대사 데이터 로드
    public static DialogueData LoadDialogue(string fileName)
    {
        TextAsset json = Resources.Load<TextAsset>($"Dialogue/{fileName}");
        return JsonUtility.FromJson<DialogueData>(json.text);
    }

    // 질병별 반응 데이터 로드
    public static class DiseaseReactionLoader
    {
        public static DiseaseReactionData LoadReactionData(string fileName)
        {
            TextAsset json = Resources.Load<TextAsset>($"Dialogue/{fileName}");
            return JsonUtility.FromJson<DiseaseReactionData>(json.text);
        }
    }

    // 인내심 감소 시 반응 데이터 로드
    public static class PatienceReactionLoader
    {
        public static PatienceReactionData LoadPatienceReactions(string fileName)
        {
            TextAsset json = Resources.Load<TextAsset>($"Dialogue/{fileName}");
            return JsonUtility.FromJson<PatienceReactionData>(json.text);
        }
    }

    // 증상 라벨(태그 텍스트) 데이터 로드
    public static class SymptomLabelLoader
    {
        public static SymptomLabelData LoadSymptomLabels(string fileName)
        {
            TextAsset json = Resources.Load<TextAsset>("Dialogue/symptom_labels");
            return JsonUtility.FromJson<SymptomLabelData>(json.text);
        }
    }
}