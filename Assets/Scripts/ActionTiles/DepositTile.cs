using UnityEngine;

[RequireComponent(typeof(TileStack))]
public class DepositTile : ActionTileBase
{
    private TileStack tileStack;

    protected override void Awake()
    {
        base.Awake();
        tileStack = GetComponent<TileStack>();
    }

    protected override void ProcessCharacter(CharacterBase character)
    {
        Debug.Log("ProcessCharacter");
        if (character == null) return;
        if (tileStack == null) return;
        if (tileStack.IsFull) return;
        Debug.Log("ProcessCharacter ok");

        Carry carry = character.GetComponent<Carry>();
        if (carry == null) return;

        CarryItemType allowedType = tileStack.AllowedItemType;
        if (allowedType == CarryItemType.None) return;

        if (!carry.TryTakeLastOfTypeFromAny(allowedType, out CarriableBase item))
            return;

        if (!tileStack.TryAdd(item))
        {
            carry.TryAdd(item);
        }
    }
}