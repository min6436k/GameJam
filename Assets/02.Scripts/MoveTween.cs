using System;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[Serializable]
public class TweenSet 
{
    [HideInInspector]
    public MoveTween parentMoveTween;
    
    public float tweenTime = 1;
    public float delay = 0;
    public Ease ease = Ease.Linear;
    public float amplitude = 1;
    public float period = 1;
    private Tweener _openTween;

    
    public void Play(Transform T, Vector3 startPos)
    {
        _openTween = T.DOMove(T.position + startPos, tweenTime).SetEase(ease,amplitude,period).SetDelay(delay).Play();
    }
    
    public void PlayFrom(Transform T, Vector3 startPos)
    {
        _openTween = T.DOMove(T.position + startPos, tweenTime).From().SetEase(ease,amplitude,period).SetDelay(delay).Play();
    }
}

[DefaultExecutionOrder(10000)]

public class MoveTween : MonoBehaviour
{
    [Tooltip("등장과 퇴장 트윈이 동일한지 여부")]
    public bool isSameEntryExit = false;

    public bool playOnAwake = true;

    public Vector3 startPos;
    public TweenSet[] Tweens = new TweenSet[2];
    private Tweener _closeTween;

    private void Start()
    {
        if(playOnAwake) Open();
    }

    public void Open()
    {
        Tweens[0].PlayFrom(transform, startPos);
    }

    public void Close()
    {
        if(isSameEntryExit) Tweens[0].Play(transform, startPos);
        else Tweens[1].Play(transform, startPos);

    }
}