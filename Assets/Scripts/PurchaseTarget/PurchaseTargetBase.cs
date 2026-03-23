using UnityEngine;
using UnityEngine.Events;

public abstract class PurchaseTargetBase : MonoBehaviour, IPurchaseTarget
{
    [Header("Purchase Info")]
    [SerializeField] private int price = 10;
    [SerializeField] private bool isRepeatable = false;

    [Header("Events")]
    [SerializeField] private UnityEvent onPurchased;

    private bool isPurchasedOnce;

    public int Price => price;
    public bool IsRepeatable => isRepeatable;

    public virtual bool CanPurchase()
    {
        if (isRepeatable)
            return true;

        return !isPurchasedOnce;
    }

    public void OnPurchased()
    {
        if (!CanPurchase())
            return;

        ApplyPurchase();

        isPurchasedOnce = true;
        onPurchased?.Invoke();
    }

    protected abstract void ApplyPurchase();
}