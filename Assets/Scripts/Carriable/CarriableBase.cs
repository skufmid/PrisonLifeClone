using DG.Tweening;
using UnityEngine;

public enum CarrySlotType
{
    Front,
    Back
}

public abstract class CarriableBase : MonoBehaviour
{
    [Header("Carry Visual")]
    [SerializeField] private float stackHeight = 0.35f;
    [SerializeField] private float moveDuration = 0.25f;
    [SerializeField] private Ease moveEase = Ease.OutCubic;

    private Tween moveTween;
    private Tween rotateTween;

    public Carry Owner { get; private set; }
    public bool IsCarried => Owner != null;
    public float StackHeight => stackHeight;

    public abstract CarrySlotType SlotType { get; }

    public virtual void OnPickedUp(Carry owner, Transform parent, Vector3 localPosition)
    {
        Owner = owner;

        transform.DOKill();

        transform.SetParent(parent, true);

        moveTween = transform
            .DOLocalMove(localPosition, moveDuration)
            .SetEase(moveEase);

        rotateTween = transform
            .DOLocalRotate(Vector3.zero, moveDuration)
            .SetEase(moveEase);
    }

    public virtual void UpdateCarryPosition(Transform parent, Vector3 localPosition)
    {
        transform.DOKill();

        if (transform.parent != parent)
            transform.SetParent(parent, true);

        moveTween = transform
            .DOLocalMove(localPosition, moveDuration)
            .SetEase(moveEase);

        rotateTween = transform
            .DOLocalRotate(Vector3.zero, moveDuration)
            .SetEase(moveEase);
    }

    public virtual void OnDropped(Vector3 worldPosition)
    {
        Owner = null;

        transform.DOKill();
        transform.SetParent(null, true);

        moveTween = transform
            .DOMove(worldPosition, moveDuration)
            .SetEase(moveEase);

        rotateTween = transform
            .DORotate(Vector3.zero, moveDuration)
            .SetEase(moveEase);
    }
}