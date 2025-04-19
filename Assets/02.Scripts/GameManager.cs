using System;
using System.Collections.Generic;
using SymptomSystem;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Camera MainCamera { get; private set; }

    public GridSystem grid;

    public Vector3 MousePos => Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);
    public Transform followPoints;

    public ItemInfoOverlay itemInfoOverlay;
    void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        MainCamera = Camera.main;
        grid.gridData = PrescriptionData.Instance.CurrentDiseaseData.Grid;
        grid.Init();
    }

    // Update is called once per frame
    public void ItemSubmit()
    {
        Symptom itemEffect = Symptom.None;
        foreach (Slot i in grid.Slots)
        {
            if (!i.IsEmpty && !i.Disable)
                itemEffect = itemEffect.Add(i.ParentSlot.InItemObj.tag);
        }
        
        PrescriptionData.Instance.Submit(itemEffect);
        PuzzleResultData.Instance.HasReturnedFromPuzzle = true;
        SceneManager.LoadScene("Dialogue");
    }
}
