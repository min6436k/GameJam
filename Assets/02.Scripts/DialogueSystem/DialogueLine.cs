using System;
using System.Collections.Generic;

[Serializable]
public class DialogueChoice
{
    public string Text; // 선택지에 표시될 텍스트
    public string Next; // 선택 시 이어지는 다음 대사 ID
    public string RequiredFlag; // 선택지가 보이기 위한 조건 플래그 (옵션)
}

[Serializable]
public class DialogueLine
{
    public string Id; // 대사의 고유 ID
    public string Speaker; // 대사의 화자 이름
    public string Text; // 대사 내용
    public string Next; // 다음 대사 ID (선택지 아닐 경우 사용)

    public bool IsChoice; // 선택지 여부
    public List<DialogueChoice> Choices; // 선택지 리스트
}