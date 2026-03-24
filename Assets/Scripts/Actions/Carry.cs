using System.Collections.Generic;
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

    [Header("Back Slot Extra")]
    [SerializeField] private Transform moneyBackAnchor;

    public override bool CountsAsMoneyInventory => true;

    public bool TryAdd(CarriableBase item)
    {
        if (item == null) return false;

        StackSlotData slot = GetSlot(item.SlotType);
        if (slot == null) return false;

        bool added = TryAddToSlot(item, slot);
        if (!added) return false;

        if (slot == backSlot) RefreshBackSlotPositions();
        return true;
    }

    public bool TryTakeLast(CarrySlotType slotType, out CarriableBase item)
    {
        StackSlotData slot = GetSlot(slotType);
        bool taken = TryTakeLastFromSlot(slot, out item);

        if (taken && slot == backSlot) RefreshBackSlotPositions();
        return taken;
    }

    public bool TryTakeLastOfType(CarrySlotType slotType, CarryItemType itemType, out CarriableBase item)
    {
        StackSlotData slot = GetSlot(slotType);
        bool taken = TryTakeLastOfTypeFromSlot(slot, itemType, out item);

        if (taken && slot == backSlot) RefreshBackSlotPositions();
        return taken;
    }

    public bool TryTakeLastOfTypeFromAny(CarryItemType itemType, out CarriableBase item)
    {
        item = null;

        if (TryTakeLastOfType(CarrySlotType.Front, itemType, out item)) return true;
        if (TryTakeLastOfType(CarrySlotType.Back, itemType, out item)) return true;

        return false;
    }

    public bool TryTakeLastFromAny(out CarriableBase item)
    {
        item = null;

        if (TryTakeLast(CarrySlotType.Front, out item)) return true;
        if (TryTakeLast(CarrySlotType.Back, out item)) return true;

        return false;
    }

    public int GetCount(CarrySlotType slotType)
    {
        return GetCount(GetSlot(slotType));
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

    public void DropLastToWorld(CarrySlotType slotType, Vector3 worldPosition)
    {
        if (TryTakeLast(slotType, out CarriableBase item))
            item.OnDropped(worldPosition);
    }

    private StackSlotData GetSlot(CarrySlotType slotType)
    {
        return slotType == CarrySlotType.Front ? frontSlot : backSlot;
    }

    private void RefreshBackSlotPositions()
    {
        if (backSlot.anchor == null) return;

        bool hasMoney = false;
        bool hasOther = false;

        for (int i = 0; i < backSlot.items.Count; i++)
        {
            CarriableBase item = backSlot.items[i];
            if (item == null) continue;

            if (item.ItemType == CarryItemType.Money) hasMoney = true;
            else hasOther = true;
        }

        if (!hasMoney || !hasOther || moneyBackAnchor == null)
        {
            UpdateSlotPositions(backSlot);
            return;
        }

        List<CarriableBase> normalItems = new();
        List<CarriableBase> moneyItems = new();
        Vector3 localEuler = GetSlotLocalRotation(backSlot);

        for (int i = 0; i < backSlot.items.Count; i++)
        {
            CarriableBase item = backSlot.items[i];
            if (item == null) continue;

            if (item.ItemType == CarryItemType.Money)
            {
                moneyItems.Add(item);
                Vector3 localPos = GetStackLocalPosition(backSlot, moneyItems, moneyItems.Count - 1);
                item.UpdateCarryPosition(moneyBackAnchor, localPos, localEuler);
            }
            else
            {
                normalItems.Add(item);
                Vector3 localPos = GetStackLocalPosition(backSlot, normalItems, normalItems.Count - 1);
                item.UpdateCarryPosition(backSlot.anchor, localPos, localEuler);
            }
        }
    }
}