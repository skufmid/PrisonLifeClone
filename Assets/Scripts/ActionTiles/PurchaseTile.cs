using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TileStack))]
public class PurchaseTile : ActionTileBase
{
    [Header("Price")]
    [SerializeField] private int price = 10;

    [Header("Deposit")]
    [SerializeField] private bool destroyMoneyVisualOnDeposit = true;

    [Header("State")]
    [SerializeField] private bool disableAfterPurchase = true;

    [Header("Events")]
    [SerializeField] private UnityEvent onProgressChanged;
    [SerializeField] private UnityEvent onPurchased;

    private TileStack tileStack;
    private int depositedAmount;
    private bool isPurchased;

    public int Price => price;
    public int CurrentAmount => depositedAmount;
    public int RemainingAmount => Mathf.Max(0, price - depositedAmount);
    public bool IsPurchased => isPurchased;

    protected override void Awake()
    {
        base.Awake();
        tileStack = GetComponent<TileStack>();
    }

    protected override void ProcessCharacter(CharacterBase character)
    {
        if (character == null) return;
        if (tileStack == null) return;
        if (isPurchased) return;
        if (depositedAmount >= price)
        {
            CompletePurchase();
            return;
        }

        Carry carry = character.GetComponent<Carry>();
        if (carry == null) return;

        if (!carry.TryTakeLastOfTypeFromAny(CarryItemType.Money, out CarriableBase moneyItem))
            return;

        bool depositSuccess = false;

        if (destroyMoneyVisualOnDeposit)
        {
            moneyItem.OnReleased();
            Destroy(moneyItem.gameObject);
            depositSuccess = true;
        }
        else
        {
            depositSuccess = tileStack.TryAdd(moneyItem);
        }

        if (!depositSuccess)
        {
            carry.TryAdd(moneyItem);
            return;
        }

        depositedAmount++;
        onProgressChanged?.Invoke();

        if (depositedAmount >= price)
        {
            CompletePurchase();
        }
    }

    private void CompletePurchase()
    {
        if (isPurchased) return;

        isPurchased = true;
        onPurchased?.Invoke();

        if (disableAfterPurchase)
            gameObject.SetActive(false);
    }
}