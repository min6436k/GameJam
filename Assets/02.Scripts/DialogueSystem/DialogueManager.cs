using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;

public class DialogueManager : MonoBehaviour
{
    [Header("UI Components")]
    public Button NextButton; // 다음 대사 버튼
    public GameObject ChoiceButtonPrefab; // 선택지 버튼 프리팹
    public Transform ChoiceContainer; // 선택지 버튼 부모 오브젝트

    public NPCManager NpcManager; // NPC 데이터 관리 클래스
    private NPCData currentNPC; // 현재 대화 중인 NPC

    private Dictionary<string, DialogueLine> dialogueMap; // 전체 대사 라인 맵
    private DialogueLine currentLine; // 현재 대화 라인

    private Dictionary<string, List<string>> symptomToReactions; // 증상별 반응 대사
    private Dictionary<string, List<(string, bool)>> symptomChoicesCacheMap = new(); // 증상 퀴즈 캐시
    private Dictionary<string, string> reactionCache = new(); // 증상 리액션 캐시
    private Dictionary<string, int> questionHistory = new(); // 질문 카운트 기록
    private List<string> patienceReactions; // 인내심 반응 대사 리스트
    private Dictionary<string, string> symptomTags; // 증상 이름에 대한 표시 텍스트

    public int MaxRepeatBeforePenalty = 2; // 반복 허용 횟수
    public string DialogueFileName = "dialogue_kr"; // 대사 JSON 파일 이름

    void Start()
    {
        LoadDialogueData(); // 대사 데이터 불러오기
        LoadReactionData(); // 증상 반응 불러오기
        LoadPatienceReactions(); // 인내심 반응 불러오기
        LoadSymptomTags(); // 증상 태그 로딩

        NpcManager.AssignUniqueDiseases(); // NPC에게 랜덤 질병 배정
        NpcManager.SelectRandomNPC(); // 랜덤한 NPC 선택
        currentNPC = NpcManager.GetSelectedNPC(); // 선택된 NPC 저장

        UIManager.Instance.SetPortrait(currentNPC.Portrait); // NPC 이미지 표시
        UIManager.Instance.SetupAffectionHearts(PatienceSystem.Instance.MaxPatience); // 하트 UI 초기화
        StartDialogue("intro"); // 대화 시작
    }

    void LoadDialogueData() // 대사 JSON 파일을 불러와 Dictionary에 저장
    {
        var data = DialogueLoader.LoadDialogue(DialogueFileName); // JSON 불러오기
        dialogueMap = data.Lines.ToDictionary(l => l.Id, l => l); // 대사 ID로 맵 구성
        NextButton.onClick.AddListener(NextLine); // 다음 대사 이벤트 연결
    }

    void LoadReactionData()  // 증상별 반응 대사 데이터를 불러와 매핑
    {
        var data = DialogueLoader.DiseaseReactionLoader.LoadReactionData("disease_reactions");
        symptomToReactions = data.Reactions.ToDictionary(r => r.Symptom, r => r.Responses); // 증상 반응 매핑
    }

    void LoadPatienceReactions() // 인내심 감소 시 출력할 짜증 대사 리스트 로드
    {
        var data = DialogueLoader.PatienceReactionLoader.LoadPatienceReactions("patience_reactions");
        patienceReactions = data.Reactions; // 인내심 반응 저장
    }

    void LoadSymptomTags() // 증상 Enum 이름과 표시용 텍스트(라벨) 연결
    {
        TextAsset json = Resources.Load<TextAsset>("Dialogue/symptom_labels");
        if (json == null) return;

        SymptomLabelData tagData = JsonUtility.FromJson<SymptomLabelData>(json.text);
        symptomTags = new Dictionary<string, string>();

        if (tagData?.Labels != null)
        {
            foreach (var entry in tagData.Labels)
                symptomTags[entry.Key] = entry.Value; // 증상 이름 → 표시 텍스트 저장
        }
    }

    public void StartDialogue(string id) // 주어진 대사 ID로 대사 시작
    {
        if (dialogueMap.TryGetValue(id, out var line))
        {
            currentLine = line;
            ShowLine(line);
        }
    }

    void ShowLine(DialogueLine line)  // 현재 대사 라인을 UI에 출력하고, 선택지가 있으면 버튼 생성
    {
        UIManager.Instance.SpeakerText.text = (line.Speaker == "환자") ? currentNPC.Name : line.Speaker;
        UIManager.Instance.DialogueText.text = line.Text;

        if (line.IsChoice && line.Choices != null)
        {
            NextButton.gameObject.SetActive(false);
            ShowChoices(line.Choices);
        }
        else
        {
            NextButton.gameObject.SetActive(true);
            UIManager.Instance.ClearChoices();
        }
    }

    void ShowChoices(List<DialogueChoice> choices) // 선택지를 UI에 표시하고 버튼 클릭 시 다음 대사로 연결
    {
        UIManager.Instance.ClearChoices();

        if (currentLine.Id == "discomfort") // 증상 퀴즈 진입
        {
            GenerateSymptomQuizChoices();
            return;
        }

        foreach (var choice in choices)
        {
            UIManager.Instance.CreateChoiceButton(choice.Text, () =>
            {
                if (choice.Text != "증상 묻기") // 증상 묻기는 카운트 제외
                {
                    TrackQuestion(choice.Text);
                    CheckPatience(choice.Text);
                }

                StartDialogue(choice.Next);
            });
        }
    }

    void CreateChoiceButton(string symptomEnumName, bool isCorrect, bool countAsQuestion = true)  // 증상 선택지 버튼을 만들고 클릭 시 정답/오답 판별 및 인내심 처리
    {
        string displayText = symptomTags != null && symptomTags.ContainsKey(symptomEnumName)
            ? symptomTags[symptomEnumName]
            : symptomEnumName;

        UIManager.Instance.CreateChoiceButton(displayText, () =>
        {
            if (countAsQuestion)
            {
                TrackQuestion(symptomEnumName); // 질문 기록
                bool patienceLost = CheckPatience(symptomEnumName); // 인내심 감소 체크
                if (patienceLost) return;
            }

            if (isCorrect)
            {
                Symptom symptom = (Symptom)Enum.Parse(typeof(Symptom), symptomEnumName);
                string reaction = GetReactionFromSymptom(symptom);
                UIManager.Instance.SetSpeaker(currentNPC.Name);
                UIManager.Instance.SetDialogue(reaction);
            }
            else
            {
                StartDialogue("wrong_answer"); // 오답 대사
                return;
            }

            UIManager.Instance.ClearChoices();
            NextButton.gameObject.SetActive(true);
            NextButton.onClick.RemoveAllListeners();
            NextButton.onClick.AddListener(() =>
            {
                StartDialogue("intro");
                NextButton.onClick.RemoveAllListeners();
                NextButton.onClick.AddListener(NextLine);
            });
        });
    }

    void GenerateSymptomQuizChoices() // 증상 퀴즈 선택지 3개(정답 1개 + 오답 2개)를 생성하여 UI에 출력
    {
        string id = currentLine.Id;

        if (symptomChoicesCacheMap.TryGetValue(id, out var cached))
        {
            foreach (var (text, isTrue) in cached)
                CreateChoiceButton(text, isTrue, true);
            return;
        }

        var all = currentNPC.Disease.Symptoms;
        var realSymptoms = SymptomUtils.SymptomHelper.Extract(all);
        List<(string, bool)> choices = new();

        if (all == Symptom.None || realSymptoms.Count == 0)
        {
            var fakePool = Enum.GetValues(typeof(Symptom)).Cast<Symptom>().Where(s => s != Symptom.None).ToList();
            while (choices.Count < 3)
            {
                string candidate = fakePool[UnityEngine.Random.Range(0, fakePool.Count)].ToString();
                if (!choices.Any(c => c.Item1 == candidate))
                    choices.Add((candidate, false));
            }
        }
        else
        {
            Symptom trueSymptom = realSymptoms[UnityEngine.Random.Range(0, realSymptoms.Count)];
            var fakePool = Enum.GetValues(typeof(Symptom)).Cast<Symptom>()
                .Where(s => s != Symptom.None && !SymptomUtils.SymptomHelper.Has(all, s)).ToList();

            List<string> fakeSymptoms = new();
            while (fakeSymptoms.Count < 2)
            {
                string candidate = fakePool[UnityEngine.Random.Range(0, fakePool.Count)].ToString();
                if (!fakeSymptoms.Contains(candidate))
                    fakeSymptoms.Add(candidate);
            }

            choices = new List<(string, bool)>
            {
                (trueSymptom.ToString(), true),
                (fakeSymptoms[0], false),
                (fakeSymptoms[1], false)
            };
        }

        choices = choices.OrderBy(x => UnityEngine.Random.value).ToList();
        symptomChoicesCacheMap[id] = choices;

        foreach (var (text, isTrue) in choices)
            CreateChoiceButton(text, isTrue, true); // 카운트 포함
    }

    void TrackQuestion(string id) // 질문을 몇 번 했는지 기록 (질문 카운터 증가)
    {
        if (!questionHistory.ContainsKey(id))
            questionHistory[id] = 0;

        questionHistory[id]++;
    }

    bool CheckPatience(string id)  // 같은 질문 반복 시 인내심 감소 조건 확인 및 반응 출력
    {
        if (id == "discomfort") return false;

        if (questionHistory[id] > MaxRepeatBeforePenalty)
        {
            if (PatienceSystem.Instance.ReducePatience())
            {
                ShowPatienceReaction();
                return true;
            }
        }

        return false;
    }

    string GetReactionFromSymptom(Symptom symptom) // 주어진 증상에 대응하는 반응 대사 반환 (없으면 기본 대사)
    {
        if (symptom == Symptom.None)
            return "잘 모르겠어요. 그냥 좀 이상한 느낌만 들어요.";

        string key = symptom.ToString();
        if (reactionCache.TryGetValue(key, out var cached))
            return cached;

        if (symptomToReactions.TryGetValue(key, out var list))
        {
            string selected = list[UnityEngine.Random.Range(0, list.Count)];
            reactionCache[key] = selected;
            return selected;
        }

        return "딱히 증상은 없는 것 같아요.";
    }

    void ShowPatienceReaction() // 인내심이 깎였을 때 짜증 반응 대사를 출력
    {
        if (patienceReactions == null || patienceReactions.Count == 0) return;

        string reaction = patienceReactions[UnityEngine.Random.Range(0, patienceReactions.Count)];
        UIManager.Instance.SetSpeaker(currentNPC.Name);
        UIManager.Instance.SetDialogue(reaction);
        UIManager.Instance.ClearChoices();

        NextButton.gameObject.SetActive(true);
        NextButton.onClick.RemoveAllListeners();
        NextButton.onClick.AddListener(() =>
        {
            StartDialogue("intro");
            NextButton.onClick.RemoveAllListeners();
            NextButton.onClick.AddListener(NextLine);
        });
    }

    void NextLine()  // 다음 대사 라인으로 진행
    {
        if (currentLine == null || string.IsNullOrEmpty(currentLine.Next)) return;
        StartDialogue(currentLine.Next);
    }
}