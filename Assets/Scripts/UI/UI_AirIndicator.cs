using UnityEngine;
using DG.Tweening;

public class UI_AirIndicator : MonoBehaviour
{
    [SerializeField] private Vector3 offset = new Vector3(0, 2f, 0);
    [SerializeField] private float rotateDuration = 1f;
    [SerializeField] private float verticalMoveHeight = 0.5f;
    [SerializeField] private float verticalMoveDuration = 0.8f;

    private Renderer renderer;
    private Vector3 startPos;

    private Transform target;
    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
        renderer = GetComponentInChildren<Renderer>();

        Hide();
    }

    private void Start()
    {
        startPos = transform.position;

        // Y축 회전
        transform.DORotate(
            new Vector3(0f, 360f, 0f),
            rotateDuration,
            RotateMode.FastBeyond360
        )
        .SetEase(Ease.Linear)
        .SetLoops(-1);

        // Y축 왕복 이동
        transform.DOMoveY(startPos.y + verticalMoveHeight, verticalMoveDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    private void LateUpdate()
    {
        if (target == null) return;

        transform.position = target.position + offset;
    }

    public void Show(Transform t)
    {
        target = t;
        renderer.enabled = true;
    }

    public void Hide()
    {
        target = null;
        renderer.enabled = false;
    }
}