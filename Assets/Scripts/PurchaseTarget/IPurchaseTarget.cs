public interface IPurchaseTarget
{
    int Price { get; }
    bool IsRepeatable { get; }
    bool CanPurchase();
    void OnPurchased();
}