using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FloatingFadeOutDOTween : MonoBehaviour
{
    public float waitTime = 1f;         // 정지 시간
    public float floatHeight = 1000f;     // 위로 이동 거리
    public float fadeDuration = 5f;     // 사라지는 시간

    private Vector3 originalPos;
    private Image image;

    void Awake()
    {
        originalPos = transform.localPosition;
        image = GetComponent<Image>();
    }

    public void Play()
    {
        // 위치/투명도 초기화
        transform.localPosition = originalPos;
        image.color = new Color(1, 1, 1, 1);
        gameObject.SetActive(true);

        // DOTween 시퀀스
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(waitTime); // 1초 대기
        seq.Append(transform.DOLocalMoveY(originalPos.y + floatHeight, fadeDuration).SetEase(Ease.OutQuad));
        seq.Join(image.DOFade(0, fadeDuration));
        seq.OnComplete(() => gameObject.SetActive(false));
    }
}