using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoOverlay : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    private Item _itemData;
    private Sequence _openTween;
    private Sequence _closeTween;
    private Image _image;
    private CanvasGroup _canvasGroup;
    private RectTransform RTransform;

    private void Start()
    {
        RTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetItemData(Item target)
    {
        _itemData = target;
    }

    public void Open(Item data)
    {
        tmp.text = data.itemName + "\n" + data.info;

        _closeTween.Kill();
        _openTween = DOTween.Sequence()
            .Join(_canvasGroup.DOFade(1f, 0.25f).SetDelay(0.15f))
            .Join(RTransform.DOScale(1, 0.35f).SetEase(Ease.OutBack, 2f, 1));
    }

    public void Close()
    {
        _openTween.Kill();
        _closeTween = DOTween.Sequence()
            .Join(RTransform.DOScale(0f, 0.3f).SetEase(Ease.InBack, 1.5f, 1))
            .Join(_canvasGroup.DOFade(0, 0.2f).SetDelay(0.2f));
    }

    public void SetPos()
    {
        RTransform.position = Input.mousePosition + (Vector3)RTransform.sizeDelta * 0.55f;
    }

    public void Update()
    {
        SetPos();
    }
}