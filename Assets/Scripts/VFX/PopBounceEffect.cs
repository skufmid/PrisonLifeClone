using DG.Tweening;
using UnityEngine;

public class PopBounceEffect : MonoBehaviour
{
    [SerializeField] private float startScale = 0.2f;
    [SerializeField] private float growDuration = 0.2f;
    [SerializeField] private float bounceDuration = 0.15f;
    [SerializeField] private float bounceScale = 1.5f;

    private Vector3 originalScale;
    private Sequence sequence;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    private void OnEnable()
    {
        Play();
    }

    public void PlayOnce()
    {
        Play();
    }

    public void Play()
    {
        sequence?.Kill();

        transform.localScale = originalScale * startScale;

        sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(originalScale * bounceScale, growDuration).SetEase(Ease.OutQuad));
        sequence.Append(transform.DOScale(originalScale, bounceDuration).SetEase(Ease.OutBounce));
    }

    private void OnDisable()
    {
        sequence?.Kill();
    }
}