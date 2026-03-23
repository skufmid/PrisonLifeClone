using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TileStack))]
public class PurchaseTile : ActionTileBase
{
    [Header("Purchase Target")]
    [SerializeField] private MonoBehaviour purchaseTargetSource;

    [Header("Deposit")]
    [SerializeField] private bool destroyMoneyVisualOnDeposit = true;

    [Header("State")]
    [SerializeField] private bool disableWhenTargetCannotPurchase = true;

    [Header("Events")]
    [SerializeField] private UnityEvent onProgressChanged;
    [SerializeField] private UnityEvent onPurchased;

    private TileStack tileStack;
    private IPurchaseTarget purchaseTarget;
    private int depositedAmount;

    public int Price => purchaseTarget?.Price ?? 0;
    public int CurrentAmount => depositedAmount;
    public int RemainingAmount => Mathf.Max(0, Price - depositedAmount);
    public bool CanPurchase => purchaseTarget != null && purchaseTarget.CanPurchase();

    protected override void Awake()
    {
        base.Awake();

        tileStack = GetComponent<TileStack>();
        purchaseTarget = purchaseTargetSource as IPurchaseTarget;
    }

    protected override void ProcessCharacter(CharacterBase character)
    {
        if (character == null) return;
        if (tileStack == null) return;
        if (purchaseTarget == null) return;
        if (!purchaseTarget.CanPurchase())
        {
            TryDisableTile();
            return;
        }

        if (depositedAmount >= Price)
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

        if (depositedAmount >= Price)
        {
            CompletePurchase();
        }
    }

    private void CompletePurchase()
    {
        if (purchaseTarget == null) return;
        if (!purchaseTarget.CanPurchase()) return;

        purchaseTarget.OnPurchased();
        onPurchased?.Invoke();

        depositedAmount = 0;
        onProgressChanged?.Invoke();

        ClearDepositedVisualsIfNeeded();

        if (!purchaseTarget.IsRepeatable)
        {
            TryDisableTile();
        }
    }

    private void ClearDepositedVisualsIfNeeded()
    {
        if (destroyMoneyVisualOnDeposit) return;
        if (tileStack == null) return;

        while (tileStack.TryTakeLast(out CarriableBase item))
        {
            if (item != null)
            {
                Destroy(item.gameObject);
            }
        }
    }

    private void TryDisableTile()
    {
        if (!disableWhenTargetCannotPurchase) return;
        if (purchaseTarget == null) return;
        if (purchaseTarget.CanPurchase()) return;

        gameObject.SetActive(false);
    }
}