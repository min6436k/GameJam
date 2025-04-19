using UnityEngine;

public class EvaluateButton : MonoBehaviour
{
    public ResultEvaluator evaluator; // 퍼즐 평가를 실행할 스크립트 (Inspector에서 연결)

    void Start()
    {
        // 퍼즐 씬에서 돌아온 경우에만 평가 실행
        if (PuzzleResultData.Instance.HasReturnedFromPuzzle)
        {
            if (evaluator != null)
            {
                evaluator.Evaluate();
                PuzzleResultData.Instance.HasReturnedFromPuzzle = false; // 한 번만 평가하도록 플래그 초기화
            }
            else
            {
                Debug.LogWarning("❗ Evaluator 연결 안 됨");
            }
        }
    }
}