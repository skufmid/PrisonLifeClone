using UnityEngine;

public class DepositTile : ActionTileBase
{
    [SerializeField] private CarrySlotType slotType;
    [SerializeField] private Transform dropPoint;

    protected override void OnCharacterEnter(CharacterBase character)
    {
        Debug.Log("OnCharacterEnter");
        Carry carry = character.GetComponent<Carry>();
        if (carry == null) return;

        carry.DropLastTo(slotType, dropPoint);
    }
}