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

    private List<GameObject> activeVisuals = new();

    private void Awake()
    {
        visualMap = new();
        foreach (var entry in visualSymptoms)
        {
            if (SymptomSystem.SymptomFlag.IsVisualSymptom(entry.symptom))
                visualMap[entry.symptom] = (entry.prefab, entry.offset);
        }
    }

    public void ShowVisualSymptoms(NPCData npcData, Transform npcUIRoot)
    {
        // ? 먼저 기존 시각 증상 삭제
        foreach (var obj in activeVisuals)
        {
            if (obj != null) Destroy(obj);
        }
        activeVisuals.Clear();

        var all = SymptomSystem.SymptomFlag.Extract(npcData.Disease.Symptoms);
        var visual = all.Where(SymptomSystem.SymptomFlag.IsVisualSymptom);

        foreach (var s in visual)
        {
            if (visualMap.TryGetValue(s, out var info) && info.prefab != null)
            {
                var obj = Instantiate(info.prefab, npcUIRoot);
                obj.transform.localPosition = info.offset;

                activeVisuals.Add(obj); // ? 리스트에 저장
            }
        }
    }

}