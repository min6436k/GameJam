using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrescriptionData : MonoBehaviour
{
    public static PrescriptionData Instance { get; private set; }

    public Dictionary<Symptom, string> SymptomData = new();

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