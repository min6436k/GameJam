using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ResultEvaluator : MonoBehaviour
{
    public NPCManager npcManager;
    public GameObject medicinePopupPrefab; // 약 이미지를 담은 프리팹
    public Transform popupSpawnRoot; // 소환 위치 (UI 상에서)

    public Sprite currentImage;

    public int Score = 0;

    public void Evaluate()
    {
        Symptom submittedResult = PrescriptionData.Instance.submitSymptom;

        // 가장 유사한 질병 찾기
        DiseaseData bestMatch = FindClosestDisease(submittedResult);

        Debug.Log($"[🔎 체크] bestMatch: {(bestMatch != null ? bestMatch.DiseaseName : "null")}");
        Debug.Log($"[🔎 체크] MedicineIcon: {(bestMatch != null && bestMatch.MedicineIcon != null ? bestMatch.MedicineIcon.name : "null")}");
        Debug.Log($"[🔎 체크] medicinePopupPrefab: {(medicinePopupPrefab != null ? medicinePopupPrefab.name : "null")}");

        if (bestMatch != null && bestMatch.MedicineIcon != null && medicinePopupPrefab != null)
        {
            GameObject popup = Instantiate(medicinePopupPrefab, popupSpawnRoot);
            popup.transform.localPosition = Vector3.zero;


            var image = popup.GetComponentInChildren<Image>();
            if (image != null)
            {
                Debug.Log($"[🖼 아이콘 적용 성공] 이미지 이름: {image.name}");
                image.sprite = bestMatch.MedicineIcon;
            }
            else
            {
                Debug.LogWarning("[❌ Image 컴포넌트 없음]");
            }

            var anim = popup.GetComponent<FloatingFadeOutDOTween>();
            if (anim != null)
                anim.Play();
            else
                Debug.LogWarning("[❌ 애니메이션 스크립트 없음]");

            Debug.Log($"💊 약 프리팹 소환: {bestMatch.DiseaseName}");
        }
        else
        {
            Debug.LogWarning("❗ 약 프리팹 또는 아이콘 누락");
        }

        // 점수 평가
        var currentNPC = npcManager.GetSelectedNPC();
        var correct = SymptomSystem.SymptomFlag.Extract(currentNPC.Disease.Symptoms);
        var submitted = SymptomSystem.SymptomFlag.Extract(submittedResult);

        bool allMatch = correct.All(submitted.Contains);
        Score += allMatch ? 1 : 0;

        Debug.Log(allMatch ? "✅ 정답" : "❌ 오답");
        Debug.Log($"📊 점수: {Score}");

        npcManager.SelectRandomNPC(UIManager.Instance.PortraitImage.transform); // 증상 이미지도 같이 표시됨
        UIManager.Instance.SetPortrait(npcManager.GetSelectedNPC().Portrait);
        PatienceSystem.Instance.ResetPatience();
        UIManager.Instance.SetupAffectionHearts(PatienceSystem.Instance.MaxPatience);

        // 게임 종료 체크
        Object.FindFirstObjectByType<GameEndChecker>()?.CheckGameResult();
    }

    private DiseaseData FindClosestDisease(Symptom submitted)
    {
        var submittedList = SymptomSystem.SymptomFlag.Extract(submitted);
        Debug.Log($"[🧾 제출된 증상] {string.Join(", ", submittedList)}");

        DiseaseData best = null;
        int maxMatch = -1;

        foreach (var disease in npcManager.AllDiseases)
        {
            var list = SymptomSystem.SymptomFlag.Extract(disease.Symptoms);
            int match = list.Count(s => submittedList.Contains(s));

            Debug.Log($"[🔍 질병: {disease.DiseaseName}] → 겹치는 증상 수: {match} / 총: {list.Count}");


            if (match > maxMatch)
            {
                best = disease;
                maxMatch = match;
            }
        }

        if (best != null)
        {
            Debug.Log($"✅ [최종 선택된 질병] {best.DiseaseName} (겹친 증상 수: {maxMatch})");
        }
        else
        {
            Debug.LogWarning("❌ 일치하는 질병이 없습니다.");
        }

        return best;
    }
}