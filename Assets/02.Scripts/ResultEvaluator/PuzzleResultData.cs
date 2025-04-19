using UnityEngine;

public class PuzzleResultData : MonoBehaviour
{
    public static PuzzleResultData Instance { get; private set; }

    public Symptom DummyResult = Symptom.RunnyNose | Symptom.Drowsy; // 예시로 미리 결과값 지정

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