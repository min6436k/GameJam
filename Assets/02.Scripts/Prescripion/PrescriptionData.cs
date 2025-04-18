using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrescriptionData : MonoBehaviour
{
    public static PrescriptionData Instance { get; private set; }

    public DiseaseData disease;
    public Symptom symptomTag;

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


    [ContextMenu("loadScene")]
    public void TestSceneLoad()
    {
        SceneManager.LoadScene("PuzzleScene");
    }
}