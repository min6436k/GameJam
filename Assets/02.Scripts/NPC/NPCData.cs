using UnityEngine;

// NPC 정보를 담는 ScriptableObject 클래스
[CreateAssetMenu(fileName = "NewNPC", menuName = "NPC/Create New NPC")]
public class NPCData : ScriptableObject
{
    public string Name;          // NPC의 이름
    public DiseaseData Disease;  // NPC가 가진 질병 정보
    public Sprite Portrait;      // NPC의 초상 이미지
}