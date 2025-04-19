using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SymptomVisualManager : MonoBehaviour
{
    [System.Serializable]
    public class SymptomVisualEntry
    {
        public Symptom symptom;
        public GameObject prefab;
        public Vector2 offset;
    }

    public List<SymptomVisualEntry> visualSymptoms = new();
    private Dictionary<Symptom, (GameObject prefab, Vector2 offset)> visualMap;

    private void Awake()
    {
        visualMap = new();
        foreach (var entry in visualSymptoms)
        {
            if (SymptomSystem.SymptomFlag.IsVisualSymptom(entry.symptom))
            {
                visualMap[entry.symptom] = (entry.prefab, entry.offset);
            }
        }
    }

    public void ShowVisualSymptoms(NPCData npcData, Transform npcUIRoot)
    {
        Debug.Log("[? ShowVisualSymptoms 호출됨]");

        var all = SymptomSystem.SymptomFlag.Extract(npcData.Disease.Symptoms);
        Debug.Log("[?? 추출된 모든 증상] " + string.Join(", ", all));

        var visual = all.Where(SymptomSystem.SymptomFlag.IsVisualSymptom).ToList();
        Debug.Log("[?? 시각 증상 필터 결과] " + string.Join(", ", visual));

        foreach (var s in visual)
        {
            if (visualMap.TryGetValue(s, out var info) && info.prefab != null)
            {
                var obj = Instantiate(info.prefab, npcUIRoot);
                obj.transform.localPosition = info.offset;
                Debug.Log($"[? 프리팹 소환됨] {s} → {obj.name} 위치: {obj.transform.localPosition}");
            }
            else
            {
                Debug.LogWarning($"[? 프리팹 없음] {s} 증상에 해당하는 프리팹을 찾을 수 없습니다.");
            }
        }
    }

}