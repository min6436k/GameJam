using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PrescriptionData : MonoBehaviour
{
    public static PrescriptionData Instance { get; private set; }

    public Dictionary<Symptom, string> SymptomData = new();

    public GameObject toggleSymptomPrefab;
    public float offsetY = 100;
    public Transform offsetPositionObj;
    private Dictionary<Symptom, Toggle> _symptomToggleMap = new();

    public bool SubmitFlag { get; private set; }
    public Symptom submitSymptom;

    public DiseaseData CurrentDiseaseData;



    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SceneManager.sceneLoaded += SetToggleUI;
    }
    
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= SetToggleUI;
    }


    private void SetToggleUI(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == "PuzzleScene")
        {
            StartCoroutine(SetToggleUICoroutine());
        }
        else
        {
            foreach (var i in _symptomToggleMap)
                Destroy(i.Value);

            _symptomToggleMap.Clear();
        }

    }

    IEnumerator SetToggleUICoroutine()
    {
        int index = 0;

        foreach (var i in SymptomData)
        {
            yield return new WaitForSeconds(0.1f);

            GameObject instance = Instantiate(toggleSymptomPrefab, transform, true);
            instance.transform.position = offsetPositionObj.position + Vector3.down * (offsetY * index++);
            instance.GetComponentInChildren<TextMeshProUGUI>().text = i.Value;
            Toggle toggle = instance.GetComponent<Toggle>();
            _symptomToggleMap[i.Key] = toggle;
        }
    }
    
    public void SetToggleOn(Symptom symptom,bool value)
    {
        if (_symptomToggleMap.TryGetValue(symptom, out Toggle toggle)) toggle.isOn = value;
    }

    public void Submit(Symptom symptom)
    {
        SubmitFlag = true;
        submitSymptom = symptom;
    }

    public void Reset()
    {
        SubmitFlag = false;
    }


    [ContextMenu("loadScene")]
    public void TestSceneLoad()
    {
        SceneManager.LoadScene("PuzzleScene");
    }
    
    [ContextMenu("DicLog")]
    public void DicLog()
    {
        foreach (var item in SymptomData)
        {
            Debug.Log($"{item.Key}: {item.Value}");
        }
    }

}