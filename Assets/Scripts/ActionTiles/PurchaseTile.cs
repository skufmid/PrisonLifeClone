using UnityEngine;

public class PurchaseTile : ActionTileBase
{
    [SerializeField] private int price = 10;
    [SerializeField] private string purchaseId;

    private int currentPaid;

    protected override void OnCharacterEnter(CharacterBase character)
    {
        Carry carry = character.GetComponent<Carry>();
        if (carry == null) return;

        int paid = carry.DropMoneyToTile(this, price - currentPaid);
        currentPaid += paid;

        Debug.Log($"구매 진행: {currentPaid}/{price}");

        if (currentPaid >= price)
        {
            CompletePurchase(character);
        }
    }

    private void CompletePurchase(CharacterBase character)
    {
        Debug.Log($"구매 완료: {purchaseId}");

        // TODO:
        // - 드릴 지급
        // - 광부 생성
        // - 업그레이드 적용
    }
}