using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class NPCManager : MonoBehaviour
{
    [Header("NPC ScriptableObject 리스트")]
    public List<NPCData> AllNPCs; // 전체 NPC 목록

    private List<NPCData> availableNPCs = new(); // 아직 사용되지 않은 NPC 리스트

    [SerializeField]
    private NPCData selectedNPC; // 현재 선택된 NPC

    [Header("전체 질병 ScriptableObject")]
    public List<DiseaseData> AllDiseases; // 모든 질병 데이터

    [Header("시각 증상 표시 관리자")]
    public SymptomVisualManager symptomVisualManager; // 증상 프리팹을 보여주는 매니저

    // 전체 NPC 리스트로 초기화
    public void ResetNPCPool()
    {
        availableNPCs = new List<NPCData>(AllNPCs);
    }

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

    // 다음 NPC 선택 후 증상 이미지 표시
    public NPCData GetNextRandomNPC(Transform symptomRootTransform = null)
    {
        if (selectedNPC != null && availableNPCs.Contains(selectedNPC))
            availableNPCs.Remove(selectedNPC);

        if (availableNPCs.Count == 0)
        {
            Debug.Log("NPC 리스트 초기화됨");
            ResetNPCPool();
        }

        int randomIndex = Random.Range(0, availableNPCs.Count);
        selectedNPC = availableNPCs[randomIndex];

        if (symptomRootTransform != null)
            ShowSymptomsOnNPC(selectedNPC, symptomRootTransform);

        return selectedNPC;
    }

    // 랜덤 NPC 선택 후 증상 표시
    public void SelectRandomNPC(Transform symptomRootTransform = null)
    {
        if (availableNPCs.Count == 0)
            ResetNPCPool();

        selectedNPC = availableNPCs[Random.Range(0, availableNPCs.Count)];

        if (symptomRootTransform != null)
        {           
            ShowSymptomsOnNPC(selectedNPC, symptomRootTransform);
        }
   

    }

    // NPC에게 시각 증상 이미지 표시
    public void ShowSymptomsOnNPC(NPCData npc, Transform npcUIRoot)
    {

        if (symptomVisualManager != null && npc != null)
        {
            symptomVisualManager.ShowVisualSymptoms(npc, npcUIRoot);
        }

    }
}