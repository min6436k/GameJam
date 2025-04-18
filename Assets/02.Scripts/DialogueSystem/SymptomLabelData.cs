using System.Collections.Generic;

// 하나의 증상 키(key)에 대한 표시용 라벨(value)을 정의하는 클래스
[System.Serializable]
public class LabelEntry
{
    public string Key;   // 증상의 Enum 이름 등 내부 식별자
    public string Value; // UI에 표시할 실제 텍스트
}

// 증상 라벨 데이터를 담는 클래스
[System.Serializable]
public class SymptomLabelData
{
    public List<LabelEntry> Labels; // 라벨 데이터 목록
}