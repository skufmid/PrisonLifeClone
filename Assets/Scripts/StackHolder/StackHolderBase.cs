using System.Collections.Generic;
using UnityEngine;

public abstract class StackHolderBase : MonoBehaviour
{
    public virtual bool CountsAsMoneyInventory => false;

    protected bool TryAddToSlot(CarriableBase item, StackSlotData slot)
    {
        if (item == null) return false;
        if (slot == null) return false;
        if (slot.anchor == null) return false;
        if (item.IsCarried) return false;
        if (slot.items.Count >= slot.MaxCapacity) return false;

        slot.items.Add(item);

        Vector3 localPos = GetStackLocalPosition(slot, slot.items, slot.items.Count - 1);
        Vector3 localEuler = GetSlotLocalRotation(slot);

        item.OnPickedUp(this, slot.anchor, localPos, localEuler);

        UpdateSlotPositions(slot);
        return true;
    }

    protected bool TryTakeLastFromSlot(StackSlotData slot, out CarriableBase item)
    {
        item = null;

        if (slot == null) return false;
        if (slot.items.Count == 0) return false;

        int lastIndex = slot.items.Count - 1;
        item = slot.items[lastIndex];
        slot.items.RemoveAt(lastIndex);

        if (item != null)
            item.OnReleased();

        UpdateSlotPositions(slot);
        return item != null;
    }

    protected bool TryTakeLastOfTypeFromSlot(StackSlotData slot, CarryItemType itemType, out CarriableBase item)
    {
        item = null;

        if (slot == null) return false;
        if (slot.items.Count == 0) return false;

        for (int i = slot.items.Count - 1; i >= 0; i--)
        {
            CarriableBase candidate = slot.items[i];
            if (candidate == null) continue;
            if (candidate.ItemType != itemType) continue;

            slot.items.RemoveAt(i);
            candidate.OnReleased();
            UpdateSlotPositions(slot);
            item = candidate;
            return true;
        }

        return false;
    }

    protected int GetCount(StackSlotData slot)
    {
        if (slot == null) return 0;
        return slot.items.Count;
    }

    protected bool IsFull(StackSlotData slot)
    {
        if (slot == null) return true;
        return slot.items.Count >= slot.MaxCapacity;
    }

    protected bool IsEmpty(StackSlotData slot)
    {
        if (slot == null) return true;
        return slot.items.Count == 0;
    }

    protected void UpdateSlotPositions(StackSlotData slot)
    {
        if (slot == null || slot.anchor == null) return;

        Vector3 localEuler = GetSlotLocalRotation(slot);

        for (int i = 0; i < slot.items.Count; i++)
        {
            CarriableBase item = slot.items[i];
            if (item == null) continue;

            Vector3 localPos = GetStackLocalPosition(slot, slot.items, i);
            item.UpdateCarryPosition(slot.anchor, localPos, localEuler);
        }
    }

    protected Vector3 GetStackLocalPosition(StackSlotData slot, List<CarriableBase> stack, int index)
    {
        int perLayer = slot.rows * slot.cols;
        int layer = index / perLayer;
        int indexInLayer = index % perLayer;

        int row = indexInLayer / slot.cols;
        int col = indexInLayer % slot.cols;

        float x = col * slot.itemSpacing.x;
        float z = row * slot.itemSpacing.z;

        if (slot.centerAlign)
        {
            float xOffset = (slot.cols - 1) * slot.itemSpacing.x * 0.5f;
            float zOffset = (slot.rows - 1) * slot.itemSpacing.z * 0.5f;
            x -= xOffset;
            z -= zOffset;
        }

        float y = layer * slot.itemSpacing.y;
        return new Vector3(x, y, z);
    }

    protected Vector3 GetSlotLocalRotation(StackSlotData slot)
    {
        if (slot == null) return Vector3.zero;
        return slot.useCustomRotation ? slot.rotationEuler : Vector3.zero;
    }
}