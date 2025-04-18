using System.Collections.Generic;
using System;

// 특정 증상에 대한 반응 대사 리스트
[Serializable]
public class DiseaseReactionEntry
{
    public string Symptom;              // 증상 이름 (enum 이름 문자열)
    public List<string> Responses;      // 해당 증상에 대한 환자의 반응 대사 리스트
}

// 모든 증상 반응 데이터 모음
[Serializable]
public class DiseaseReactionData
{
    public List<DiseaseReactionEntry> Reactions; // 증상별 반응 데이터 리스트
}