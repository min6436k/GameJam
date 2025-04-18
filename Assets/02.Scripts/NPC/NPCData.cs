using UnityEngine;


[CreateAssetMenu(fileName = "NewNPC", menuName = "NPC/Create New NPC")]
public class NPCData : ScriptableObject
{
    public string Name;
    public DiseaseData Disease;
    public Sprite Portrait;
}