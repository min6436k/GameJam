using UnityEngine;

public class PuzzleResultData : MonoBehaviour
{
    public static PuzzleResultData Instance { get; private set; }
    public bool HasReturnedFromPuzzle = false;

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