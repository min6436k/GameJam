using System;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    public static OptionUI Instance;
    public AudioMixer audioMixer;

    private float _screenY;
    private RectTransform RTransforn;
    private Tweener _downTween;
    private Tweener _upTween;

    private float[] _volumes = new float[3];
    void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(this.gameObject);
            DontDestroyOnLoad(this.gameObject);
        }

        _screenY = Screen.height*1.5f;

    }

    void Start()
    {
        RTransforn = GetComponent<RectTransform>();
        _downTween = RTransforn.DOAnchorPosY(0, 0.9f)
            .SetAutoKill(false).Pause().SetEase(Ease.OutElastic, -1, 1.1f);
        
        _upTween = RTransforn.DOAnchorPosY(_screenY, 0.6f)
            .SetAutoKill(false).Pause().SetEase(Ease.InBack,1.2f,20f)
            .OnComplete(()=>gameObject.SetActive(false));

        RTransforn.anchoredPosition = new Vector2(0,_screenY);
    }

    public void OpenUI()
    {
        _downTween.Restart();
        gameObject.SetActive(true);
    }
    
    public void CloseUI() =>
        _upTween.Restart();

    public void SetMasterVolume(float volume) =>
        audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
    
    public void SetBGMVolume(float volume) =>
        audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
    
    public void SetSFXVolume(float volume) =>
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
}
