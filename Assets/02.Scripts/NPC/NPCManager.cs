using UnityEngine;
using System.Collections.Generic;

public class NPCManager : MonoBehaviour
{
    [Header("NPC ScriptableObject 리스트")]
    public List<NPCData> AllNPCs; // 사용할 NPC 목록 (플레이하면서 줄어듦)

    [SerializeField]
    private NPCData selectedNPC; // 현재 선택된 NPC

    [Header("전체 질병 ScriptableObject")]
    public List<DiseaseData> AllDiseases;

    [Header("시각 증상 표시 관리자")]
    public SymptomVisualManager symptomVisualManager;

    // NPC에게 중복 없이 질병을 랜덤하게 배정
    public void AssignUniqueDiseases()
    {
        List<DiseaseData> pool = new List<DiseaseData>(AllDiseases);

        foreach (var npc in AllNPCs)
        {
            if (pool.Count == 0) break;
            int index = Random.Range(0, pool.Count);
            npc.Disease = pool[index];
            pool.RemoveAt(index);
        }
    }

    // 현재 선택된 NPC 반환
    public NPCData GetSelectedNPC()
    {
        return selectedNPC;
    }

    // 다음 NPC 랜덤 선택 → AllNPCs에서 제거
    public void SelectRandomNPC(Transform symptomRootTransform = null)
    {
        if (AllNPCs.Count == 0)
        {
            Debug.LogWarning("모든 NPC를 사용했습니다.");
            selectedNPC = null;
            return;
        }

        int randomIndex = Random.Range(0, AllNPCs.Count);
        selectedNPC = AllNPCs[randomIndex];
        AllNPCs.RemoveAt(randomIndex); // ✅ 리스트에서 제거

        Debug.Log($"[🎯 선택된 NPC] {selectedNPC.Name}");

        if (symptomRootTransform != null)
            ShowSymptomsOnNPC(selectedNPC, symptomRootTransform);
    }

    // 시각 증상 보여주기
    public void ShowSymptomsOnNPC(NPCData npc, Transform npcUIRoot)
    {
        if (symptomVisualManager != null && npc != null)
            symptomVisualManager.ShowVisualSymptoms(npc, npcUIRoot);
    }

    // 더 이상 NPC가 남아있는지 여부
    public bool IsAllNPCUsed()
    {
        return AllNPCs.Count == 0;
    }
}