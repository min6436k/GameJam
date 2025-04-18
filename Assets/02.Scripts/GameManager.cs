using System;
using System.Collections.Generic;
using SymptomSystem;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Camera MainCamera { get; private set; }

    public GridSystem grid;

    public Vector3 MousePos => Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);
    public Transform followPoints;
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
    }

    // Update is called once per frame
    public void ItemSubmit()
    {
        SymptomTag itemEffect = SymptomTag.None;
        foreach (Slot i in grid.Slots)
        {
            if (!i.IsEmpty && !i.Disable)
                itemEffect = itemEffect.Add(i.ParentSlot.InItemObj.tag);
        }
        
        Debug.Log(itemEffect);
    }
}
