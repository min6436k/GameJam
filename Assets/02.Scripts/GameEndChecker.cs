using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameEndChecker : MonoBehaviour
{   

    public NPCManager npcManager; // NPC 리스트와 현재 NPC 접근
    public PatienceSystem patienceSystem;
    public GameObject Panel;
    public TextMeshProUGUI text1;
    public TextMeshProUGUI text2;
    public ResultEvaluator result;

    public void CheckGameResult()
    {
        if (patienceSystem.IsOutOfPatience)
        {
            PatienceSystem.Instance.ReducePatience();
            // TODO: 패배 UI 호출 등 처리
        }
        else if (npcManager.IsAllNPCUsed()) // 이 함수는 따로 만들어야 함
        {
            Debug.Log(" 게임 승리 - 모든 NPC 처방 완료");
            Result();
        }
        else
        {
            Debug.Log("게임 계속 진행 중");
        }
    }
    
    public void Result()
    {   
        Panel.SetActive(true);
        Panel.GetComponent<MoveTween>().Open();
        text1.text = PatienceSystem.Instance.CurrentPatience.ToString();
        text2.text = result.Score.ToString();
    }
}
