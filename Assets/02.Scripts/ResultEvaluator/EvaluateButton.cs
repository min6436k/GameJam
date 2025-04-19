using UnityEngine;
using UnityEngine.UI;

public class EvaluateButton : MonoBehaviour
{
    public ResultEvaluator evaluator; // 결과 평가 스크립트 (Inspector에서 연결)

    public void TestStart()
    {
        if(evaluator != null)
            evaluator.Evaluate();
        else
            Debug.LogWarning("버튼 또는 평가기 연결 안됨");
    }
}