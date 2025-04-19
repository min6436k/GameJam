using System;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    public static OptionUI Instance;
    public AudioMixer audioMixer;
    public GameObject buttonObj;

    private float _screenY;
    private RectTransform RTransform;
    private Tweener _downTween;
    private Tweener _upTween;

    private float[] _volumes = new float[3];
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(transform.parent.gameObject);
        }
        else
        {
            Destroy(transform.parent);
        }

        _screenY = Screen.height*1.5f;

    }

    void Start()
    {
        SceneManager.sceneLoaded += (scene, mode) =>
        {
            if (buttonObj != null && SceneManager.GetActiveScene().name != "Main") buttonObj.SetActive(true);
        };
        
        RTransform = GetComponent<RectTransform>();
        _downTween = RTransform.DOAnchorPosY(0, 0.9f)
            .SetAutoKill(false).Pause().SetEase(Ease.OutElastic, -1, 1.1f);
        
        _upTween = RTransform.DOAnchorPosY(_screenY, 0.6f)
            .SetAutoKill(false).Pause().SetEase(Ease.InBack,1.2f,20f)
            .OnComplete(()=>gameObject.SetActive(false));

        RTransform.anchoredPosition = new Vector2(0,_screenY);
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
    
    public void ExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
