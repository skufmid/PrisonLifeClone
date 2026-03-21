using UnityEngine;

public class TileStack : StackHolderBase
{
    [Header("Tile Slot")]
    [SerializeField]
    private StackSlotData tileSlot = new StackSlotData
    {
        slotType = CarrySlotType.Tile
    };

    [Header("Accept Settings")]
    [SerializeField] private CarryItemType allowedItemType = CarryItemType.None;

    public CarryItemType AllowedItemType => allowedItemType;
    public int Count => GetCount(tileSlot);
    public bool IsFull => base.IsFull(tileSlot);
    public bool IsEmpty => base.IsEmpty(tileSlot);

    public bool CanAccept(CarriableBase item)
    {
        if (item == null) return false;
        if (allowedItemType == CarryItemType.None) return false;
        if (item.ItemType != allowedItemType) return false;
        if (IsFull) return false;

        return true;
    }

    public bool TryAdd(CarriableBase item)
    {
        if (!CanAccept(item)) return false;
        return TryAddToSlot(item, tileSlot);
    }

    public bool TryTakeLast(out CarriableBase item)
    {
        return TryTakeLastFromSlot(tileSlot, out item);
    }

    public bool TryTakeLastOfType(CarryItemType itemType, out CarriableBase item)
    {
        return TryTakeLastOfTypeFromSlot(tileSlot, itemType, out item);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (tileSlot == null || tileSlot.anchor == null) return;

        Gizmos.color = Color.yellow;

        int capacity = tileSlot.MaxCapacity;
        for (int i = 0; i < capacity; i++)
        {
            Vector3 localPos = GetStackLocalPosition(tileSlot, tileSlot.items, i);
            Vector3 worldPos = tileSlot.anchor.TransformPoint(localPos);
            Gizmos.DrawWireCube(worldPos, Vector3.one * 0.15f);
        }
    }
#endif
}