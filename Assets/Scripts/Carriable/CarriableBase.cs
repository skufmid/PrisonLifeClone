using DG.Tweening;
using UnityEngine;

public enum CarrySlotType
{
    Front,
    Back,
    Tile
}

public enum CarryItemType
{
    None,
    Rock,
    Handcuff,
    Money
}

public abstract class CarriableBase : MonoBehaviour
{
    [Header("Item Info")]
    [SerializeField] private CarryItemType itemType = CarryItemType.None;

    [Header("Carry Visual")]
    [SerializeField] private float stackHeight = 0.35f;
    [SerializeField] private float moveDuration = 0.25f;
    [SerializeField] private Ease moveEase = Ease.OutCubic;

    private Tween moveTween;
    private Tween rotateTween;

    public StackHolderBase Owner { get; private set; }
    public bool IsCarried => Owner != null;
    public float StackHeight => stackHeight;
    public CarryItemType ItemType => itemType;

    public abstract CarrySlotType SlotType { get; }

    public virtual void OnPickedUp(StackHolderBase owner, Transform parent, Vector3 localPosition, Vector3 localEulerAngles)
    {
        Owner = owner;

        transform.DOKill();
        transform.SetParent(parent, true);

        moveTween = transform
            .DOLocalMove(localPosition, moveDuration)
            .SetEase(moveEase);

        rotateTween = transform
            .DOLocalRotate(localEulerAngles, moveDuration)
            .SetEase(moveEase);
    }

    public virtual void UpdateCarryPosition(Transform parent, Vector3 localPosition, Vector3 localEulerAngles)
    {
        transform.DOKill();

        if (transform.parent != parent)
            transform.SetParent(parent, true);

        moveTween = transform
            .DOLocalMove(localPosition, moveDuration)
            .SetEase(moveEase);

        rotateTween = transform
            .DOLocalRotate(localEulerAngles, moveDuration)
            .SetEase(moveEase);
    }

    public virtual void OnReleased()
    {
        Owner = null;
        transform.DOKill();
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