using UnityEngine;
using UnityEngine.UI;

public class SymptomToggleItem : MonoBehaviour
{
    public Toggle Toggle; // 연결된 토글 UI 요소
    public string SymptomName; // 이 토글이 대표하는 증상 이름

    void Start()
    {
        Toggle.onValueChanged.AddListener(OnToggleChanged); // 토글 상태 변경 시 이벤트 연결
    }

    void OnToggleChanged(bool isOn)
    {
        if (isOn)
        {
            // 체크되었을 때 리스트에 증상이 없으면 추가
            if (!PrescriptionData.Instance.CheckedSymptoms.Contains(SymptomName))
                PrescriptionData.Instance.CheckedSymptoms.Add(SymptomName);
        }
        else
        {
            // 체크 해제되면 리스트에서 제거
            PrescriptionData.Instance.CheckedSymptoms.Remove(SymptomName);
        }
    }
}