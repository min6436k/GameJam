using UnityEngine;

public class GameEndChecker : MonoBehaviour
{
    public NPCManager npcManager; // NPC 리스트와 현재 NPC 접근
    public PatienceSystem patienceSystem; // 인내심 시스템

    public void CheckGameResult()
    {
        if (patienceSystem.IsOutOfPatience)
        {
            Debug.Log("❌ 게임 패배 - 인내심 소진");
            // TODO: 패배 UI 호출 등 처리
        }
        else if (npcManager.IsAllNPCUsed()) // 이 함수는 따로 만들어야 함
        {
            Debug.Log("🏆 게임 승리 - 모든 NPC 처방 완료");
            // TODO: 승리 UI 호출 등 처리
        }
        else
        {
            Debug.Log("🔄 게임 계속 진행 중");
        }
    }
}
