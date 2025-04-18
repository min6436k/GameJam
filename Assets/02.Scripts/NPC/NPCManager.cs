using UnityEngine;
using System.Collections.Generic;

public class NPCManager : MonoBehaviour
{
    [Header("NPC ScriptableObject 리스트")]
    public List<NPCData> AllNPCs; // 전체 NPC ScriptableObject 목록

    private List<NPCData> availableNPCs = new(); // 현재 선택 가능한 NPC 리스트
    private NPCData selectedNPC; // 현재 선택된 NPC

    [Header("전체 질병 ScriptableObject")]
    public List<DiseaseData> AllDiseases; // 모든 질병 ScriptableObject 목록

    // availableNPCs 리스트를 AllNPCs로 초기화
    public void ResetNPCPool()
    {
        availableNPCs = new List<NPCData>(AllNPCs);
    }

    // NPC에게 중복 없이 질병을 랜덤하게 할당
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

    // 현재 선택된 NPC를 제거하고 다음 랜덤 NPC 선택
    public NPCData GetNextRandomNPC()
    {
        if (selectedNPC != null && availableNPCs.Contains(selectedNPC))
            availableNPCs.Remove(selectedNPC);

        if (availableNPCs.Count == 0)
        {
            Debug.Log("초기화");
            ResetNPCPool();
        }

        int randomIndex = Random.Range(0, availableNPCs.Count);
        selectedNPC = availableNPCs[randomIndex];
        return selectedNPC;
    }

    // 랜덤한 NPC를 선택하여 selectedNPC에 저장
    public void SelectRandomNPC()
    {
        if (availableNPCs.Count == 0)
            ResetNPCPool();

        selectedNPC = availableNPCs[Random.Range(0, availableNPCs.Count)];
    }
}