using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; } // 싱글톤 인스턴스

    [Header("Dialogue UI")]
    public TMP_Text SpeakerText; // 화자 이름 텍스트
    public TMP_Text DialogueText; // 대사 내용 텍스트
    public Transform ChoiceContainer; // 선택지 버튼을 담을 부모 오브젝트
    public GameObject ChoiceButtonPrefab; // 선택지 버튼 프리팹

    [Header("Patience UI")]
    public Transform HeartContainer; // 하트 UI를 담을 부모 오브젝트
    public GameObject HeartPrefab; // 하트 프리팹

    private List<GameObject> heartObjects = new(); // 생성된 하트 오브젝트 목록

    public Image PortraitImage; // NPC 초상화 이미지

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SetSpeaker(string speaker)
    {
        // 화자 이름 출력
        if (SpeakerText != null)
        {
            Debug.Log($"[UI] 화자 이름 설정: {speaker}");
            SpeakerText.text = speaker;
        }
    }

    public void SetDialogue(string text)
    {
        // 대사 내용 출력
        if (DialogueText != null)
        {
            Debug.Log($"[UI] 대사 텍스트 설정: {text}");
            DialogueText.text = text;
        }
    }

    public void ClearChoices()
    {
        // 선택지 버튼 전부 삭제
        foreach (Transform child in ChoiceContainer)
        {
            Destroy(child.gameObject);
        }
    }

    public void CreateChoiceButton(string text, Action onClick)
    {
        // 선택지 버튼 생성 및 클릭 이벤트 연결
        var buttonObj = Instantiate(ChoiceButtonPrefab, ChoiceContainer);
        var button = buttonObj.GetComponent<Button>();
        var buttonText = buttonObj.GetComponentInChildren<TMP_Text>();

        buttonText.text = text;
        button.onClick.AddListener(() => onClick.Invoke());
    }

    public void SetupHearts(int count)
    {
        // 하트 초기화 및 지정 개수만큼 생성
        foreach (Transform child in HeartContainer)
        {
            Destroy(child.gameObject);
        }

        heartObjects.Clear();

        for (int i = 0; i < count; i++)
        {
            GameObject h = Instantiate(HeartPrefab, HeartContainer);
            heartObjects.Add(h);
        }
    }

    public void UpdateHearts(int current)
    {
        // 현재 인내심 수치에 따라 하트 표시 업데이트
        for (int i = 0; i < heartObjects.Count; i++)
        {
            heartObjects[i].SetActive(i < current);
        }
    }

    public void UpdateAffectionHearts(int affection)
    {
        // 현재 호감도 수치에 따라 하트 표시 업데이트
        for (int i = 0; i < heartObjects.Count; i++)
        {
            heartObjects[i].SetActive(i < affection);
        }
    }

    public void SetupAffectionHearts(int count)
    {
        // 호감도 하트를 초기화하고 다시 생성
        foreach (Transform child in HeartContainer)
            Destroy(child.gameObject);

        heartObjects.Clear();

        for (int i = 0; i < count; i++)
        {
            var heart = Instantiate(HeartPrefab, HeartContainer);
            heartObjects.Add(heart);
        }
    }

    public void SetPortrait(Sprite sprite)
    {
        // NPC 초상화 이미지 적용 및 표시
        PortraitImage.sprite = sprite;
        PortraitImage.enabled = sprite != null;
    }
}