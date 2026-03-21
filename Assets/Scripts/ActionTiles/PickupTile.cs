using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupTile : ActionTileBase
{
    [Header("Pickup Settings")]
    [SerializeField] private float pickupInterval = 0.1f;
    [SerializeField] private bool pickFrontFirst = true;

    private readonly List<CarriableBase> itemsOnTile = new();
    private Coroutine pickupCoroutine;

    protected override void OnCharacterEnter(CharacterBase character)
    {
        if (pickupCoroutine != null)
            StopCoroutine(pickupCoroutine);

        pickupCoroutine = StartCoroutine(CoPickup(character));
    }

    private void OnTriggerExit(Collider other)
    {
        CharacterBase character = other.GetComponent<CharacterBase>();
        if (character == null) return;

        if (pickupCoroutine != null)
        {
            StopCoroutine(pickupCoroutine);
            pickupCoroutine = null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        CarriableBase item = other.GetComponent<CarriableBase>();
        if (item == null) return;

        if (!itemsOnTile.Contains(item) && !item.IsCarried)
        {
            itemsOnTile.Add(item);
        }
    }

    private IEnumerator CoPickup(CharacterBase character)
    {
        Carry carry = character.GetComponent<Carry>();
        if (carry == null)
            yield break;

        while (true)
        {
            CleanupNulls();

            CarriableBase nextItem = GetNextPickableItem();
            if (nextItem == null)
            {
                pickupCoroutine = null;
                yield break;
            }

            bool success = carry.TryStart(nextItem);

            if (success)
            {
                itemsOnTile.Remove(nextItem);
            }
            else
            {
                // 들 수 없는 상태면 종료
                pickupCoroutine = null;
                yield break;
            }

            yield return new WaitForSeconds(pickupInterval);
        }
    }

    private CarriableBase GetNextPickableItem()
    {
        if (itemsOnTile.Count == 0)
            return null;

        if (pickFrontFirst)
        {
            for (int i = 0; i < itemsOnTile.Count; i++)
            {
                if (itemsOnTile[i] == null || itemsOnTile[i].IsCarried)
                    continue;

                if (itemsOnTile[i].SlotType == CarrySlotType.Front)
                    return itemsOnTile[i];
            }
        }

        for (int i = 0; i < itemsOnTile.Count; i++)
        {
            if (itemsOnTile[i] == null || itemsOnTile[i].IsCarried)
                continue;

            return itemsOnTile[i];
        }

        return null;
    }

    private void CleanupNulls()
    {
        for (int i = itemsOnTile.Count - 1; i >= 0; i--)
        {
            if (itemsOnTile[i] == null || itemsOnTile[i].IsCarried)
            {
                itemsOnTile.RemoveAt(i);
            }
        }
    }
}