using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carry : ActionBase<CarriableBase>
{
    [Header("Carry Points")]
    [SerializeField] private Transform frontPosition;
    [SerializeField] private Transform backPosition;

    [Header("Capacity")]
    [SerializeField] private int frontCapacity = 5;
    [SerializeField] private int backCapacity = 10;

    [Header("Drop Settings")]
    [SerializeField] private float dropSpacing = 0.25f;
    [SerializeField] private float sideOffset = 0.35f;

    private readonly List<CarriableBase> frontStack = new();
    private readonly List<CarriableBase> backStack = new();

    protected override bool CanStart(CarriableBase target)
    {
        if (target == null) return false;
        if (target.IsCarried) return false;

        List<CarriableBase> stack = GetStack(target.SlotType);
        int capacity = GetCapacity(target.SlotType);

        return stack.Count < capacity;
    }

    protected override void OnStarted()
    {
        if (target == null)
        {
            StopAction();
            return;
        }

        AddToStack(target);
    }

    protected override IEnumerator CoAction()
    {
        yield return null;
        StopAction();
    }

    private void AddToStack(CarriableBase item)
    {
        List<CarriableBase> stack = GetStack(item.SlotType);
        Transform anchor = GetAnchor(item.SlotType);

        stack.Add(item);

        Vector3 localPos = GetStackLocalPosition(stack, stack.Count - 1);
        item.OnPickedUp(this, anchor, localPos);

        UpdateStackPositions(item.SlotType);
    }

    public bool DropLast(CarrySlotType slotType)
    {
        List<CarriableBase> stack = GetStack(slotType);
        if (stack.Count == 0) return false;

        int lastIndex = stack.Count - 1;
        CarriableBase item = stack[lastIndex];
        stack.RemoveAt(lastIndex);

        Vector3 dropPosition = GetDropPosition(slotType, lastIndex);
        item.OnDropped(dropPosition);

        UpdateStackPositions(slotType);
        return true;
    }

    public bool DropLastTo(CarrySlotType slotType, Transform targetPoint)
    {
        List<CarriableBase> stack = GetStack(slotType);
        if (stack.Count == 0) return false;
        if (targetPoint == null) return false;

        int lastIndex = stack.Count - 1;
        CarriableBase item = stack[lastIndex];
        stack.RemoveAt(lastIndex);

        item.OnDropped(targetPoint.position);

        UpdateStackPositions(slotType);
        return true;
    }

    public int DropMoneyToTile(PurchaseTile tile, int maxAmount)
    {
        if (tile == null || maxAmount <= 0)
            return 0;

        int deposited = 0;

        List<CarriableBase> stack = backStack;

        for (int i = stack.Count - 1; i >= 0; i--)
        {
            if (deposited >= maxAmount)
                break;

            if (stack[i] is not Money money)
                continue;

            money.OnDropped(tile.transform.position);
            stack.RemoveAt(i);
            deposited++;
        }

        UpdateStackPositions(CarrySlotType.Back);
        return deposited;
    }

    public int GetCount(CarrySlotType slotType)
    {
        return GetStack(slotType).Count;
    }

    private void UpdateStackPositions(CarrySlotType slotType)
    {
        List<CarriableBase> stack = GetStack(slotType);
        Transform anchor = GetAnchor(slotType);

        for (int i = 0; i < stack.Count; i++)
        {
            Vector3 localPos = GetStackLocalPosition(stack, i);
            stack[i].UpdateCarryPosition(anchor, localPos);
        }
    }

    private Vector3 GetStackLocalPosition(List<CarriableBase> stack, int index)
    {
        float y = 0f;

        for (int i = 0; i < index; i++)
        {
            y += stack[i].StackHeight;
        }

        return new Vector3(0f, y, 0f);
    }

    private Vector3 GetDropPosition(CarrySlotType slotType, int order)
    {
        Transform anchor = GetAnchor(slotType);

        Vector3 horizontalOffset =
            slotType == CarrySlotType.Front
            ? transform.right * sideOffset
            : -transform.right * sideOffset;

        Vector3 depthOffset = transform.forward * (order * dropSpacing);

        return anchor.position + horizontalOffset + depthOffset;
    }

    private List<CarriableBase> GetStack(CarrySlotType slotType)
    {
        return slotType == CarrySlotType.Front ? frontStack : backStack;
    }

    private Transform GetAnchor(CarrySlotType slotType)
    {
        return slotType == CarrySlotType.Front ? frontPosition : backPosition;
    }

    private int GetCapacity(CarrySlotType slotType)
    {
        return slotType == CarrySlotType.Front ? frontCapacity : backCapacity;
    }
}