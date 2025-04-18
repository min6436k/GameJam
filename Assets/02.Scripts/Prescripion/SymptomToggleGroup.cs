using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class SymptomToggleGroup : MonoBehaviour
{
    [System.Serializable]
    public class SymptomToggleEntry
    {
        public Toggle toggle;       
        public Symptom symptom;       
        public string displayText;   
    }

    public List<SymptomToggleEntry> symptomToggles = new();

    void Start()
    {
        foreach (var entry in symptomToggles)
        {
            entry.toggle.onValueChanged.AddListener((isOn) =>
            {
                OnToggleChanged(entry, isOn);
            });
        }
    }

    void OnToggleChanged(SymptomToggleEntry entry, bool isOn)
    {
        if (isOn)
        {
            if (!PrescriptionData.Instance.SymptomData.ContainsKey(entry.symptom))
                PrescriptionData.Instance.SymptomData[entry.symptom] = entry.displayText;
            Debug.Log($"현재 저장된 증상 : {entry.symptom}");
        }
        else
        {
            if (PrescriptionData.Instance.SymptomData.ContainsKey(entry.symptom))
                PrescriptionData.Instance.SymptomData.Remove(entry.symptom);
        }
    }
}