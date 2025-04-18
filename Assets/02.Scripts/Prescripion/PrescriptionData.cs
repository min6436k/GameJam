using System.Collections.Generic;
using UnityEngine;

public class PrescriptionData : MonoBehaviour
{
    public static PrescriptionData Instance { get; private set; } // 싱글톤 인스턴스

    public List<string> CheckedSymptoms = new(); // 체크된 증상 리스트

    private void Awake()
    {
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}