using System;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(10000)]
public class FadeTween : MonoBehaviour
{
    public bool playOnAwake = true;

    [Range(0f, 1f)]
    public float endAlpha;

    public float tweenTime = 1;
    public float delay = 0;
    public Ease ease = Ease.Linear;
    private Tweener Tween;
    private Image _image;
    private void Start()
    {
        _image = GetComponent<Image>();
        if(playOnAwake) FadeIn();
    }

    public void FadeIn()
    {
        _image.color = Color.clear;
        Tween = _image.DOFade(endAlpha, tweenTime).SetDelay(delay).Play();
    }

    public void FadeOut()
    {
        Tween = _image.DOFade(endAlpha, tweenTime).From().SetDelay(delay).Play();
    }
}
