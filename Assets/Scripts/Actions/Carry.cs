using System.Collections;
using UnityEngine;

public class Carry : StackHolderBase
{
    [Header("Front Slot")]
    [SerializeField]
    private StackSlotData frontSlot = new StackSlotData
    {
        slotType = CarrySlotType.Front
    };

    [Header("Back Slot")]
    [SerializeField]
    private StackSlotData backSlot = new StackSlotData
    {
        slotType = CarrySlotType.Back
    };
    public override bool CountsAsMoneyInventory => true;

    public bool TryAdd(CarriableBase item)
    {
        if (item == null) return false;

        StackSlotData slot = GetSlot(item.SlotType);
        if (slot == null) return false;

        return TryAddToSlot(item, slot);
    }

    public bool TryTakeLast(CarrySlotType slotType, out CarriableBase item)
    {
        return TryTakeLastFromSlot(GetSlot(slotType), out item);
    }

    public bool TryTakeLastOfType(CarrySlotType slotType, CarryItemType itemType, out CarriableBase item)
    {
        return TryTakeLastOfTypeFromSlot(GetSlot(slotType), itemType, out item);
    }

    public bool TryTakeLastOfTypeFromAny(CarryItemType itemType, out CarriableBase item)
    {
        item = null;

        if (TryTakeLastOfType(CarrySlotType.Front, itemType, out item))
            return true;

        if (TryTakeLastOfType(CarrySlotType.Back, itemType, out item))
            return true;

        return false;
    }

    public bool HasItem(CarryItemType itemType)
    {
        for (int i = frontSlot.items.Count - 1; i >= 0; i--)
        {
            if (frontSlot.items[i] != null && frontSlot.items[i].ItemType == itemType)
                return true;
        }

        for (int i = backSlot.items.Count - 1; i >= 0; i--)
        {
            if (backSlot.items[i] != null && backSlot.items[i].ItemType == itemType)
                return true;
        }

        return false;
    }

    private StackSlotData GetSlot(CarrySlotType slotType)
    {
        return slotType == CarrySlotType.Front ? frontSlot : backSlot;
    }
}