using UnityEngine;

public class MoneyCarriable : CarriableBase
{
    private const int MoneyValue = 5;
    private bool isCounted;

    public override CarrySlotType SlotType => CarrySlotType.Front;

    public override void OnPickedUp(
        StackHolderBase owner,
        Transform parent,
        Vector3 localPosition,
        Vector3 localEulerAngles)
    {
        base.OnPickedUp(owner, parent, localPosition, localEulerAngles);

        if (owner != null && owner.CountsAsMoneyInventory && !isCounted)
        {
            GameManager.Instance.PlusMoney(MoneyValue);
            isCounted = true;
        }
    }

    public override void OnReleased()
    {
        if (isCounted)
        {
            GameManager.Instance.UseMoney(MoneyValue);
            isCounted = false;
        }

        base.OnReleased();
    }

    public override void OnDropped(Vector3 worldPosition)
    {
        if (isCounted)
        {
            GameManager.Instance.UseMoney(MoneyValue);
            isCounted = false;
        }

        base.OnDropped(worldPosition);
    }
}