using UnityEngine;

[RequireComponent(typeof(TileStack))]
public class PickupTile : ActionTileBase
{
    private TileStack tileStack;

    protected override void Awake()
    {
        base.Awake();
        tileStack = GetComponent<TileStack>();
    }

    protected override void ProcessCharacter(CharacterBase character)
    {
        if (character == null) return;
        if (tileStack == null) return;
        if (tileStack.IsEmpty) return;

        Carry carry = character.GetComponent<Carry>();
        if (carry == null) return;

        if (!tileStack.TryTakeLast(out CarriableBase item))
            return;

        if (!carry.TryAdd(item))
        {
            tileStack.TryAdd(item);
        }
    }
}