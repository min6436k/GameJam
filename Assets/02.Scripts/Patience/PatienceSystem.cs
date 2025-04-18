using UnityEngine;

public class PatienceSystem : MonoBehaviour
{
    public static PatienceSystem Instance { get; private set; } // 싱글톤 인스턴스

    [Header("인내심 설정")]
    public int MaxPatience = 5; // 최대 인내심 수치
    public int CurrentPatience; // 현재 인내심 수치

    public bool IsOutOfPatience => CurrentPatience <= 0; // 인내심이 바닥났는지 여부

    private void Awake()
    {
        // 싱글톤 초기화
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); 
    }

    private void Start()
    {
        ResetPatience();
    }

    // 인내심을 최대치로 리셋하고 UI 초기화
    public void ResetPatience()
    {
        CurrentPatience = MaxPatience;
        UIManager.Instance.SetupHearts(MaxPatience);
    }

    // 인내심을 1 감소시키고, UI 갱신. 감소 시 true 반환
    public bool ReducePatience()
    {
        if (CurrentPatience <= 0) return false;

        CurrentPatience--;
        UIManager.Instance.UpdateHearts(CurrentPatience);
        Debug.Log($"남은 인내심: {CurrentPatience}");
        return true;
    }
}