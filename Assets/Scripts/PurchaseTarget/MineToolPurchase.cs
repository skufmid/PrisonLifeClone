using UnityEngine;
using UnityEngine.Events;

public class MineToolPurchase : MonoBehaviour
{
    [Header("Purchase Target")]
    [SerializeField] private MineTool mineTool;

    [Header("Apply Target")]
    [SerializeField] private Mine[] targetMines;

    [Header("Options")]
    [SerializeField] private bool equipImmediately = true;
    [SerializeField] private bool purchaseOnlyOnce = true;

    [Header("Optional Toggle")]
    [SerializeField] private GameObject[] activateOnPurchase;
    [SerializeField] private GameObject[] deactivateOnPurchase;

    [Header("Events")]
    [SerializeField] private UnityEvent onPurchased;

    private bool isPurchased;

    public MineTool PurchasedTool => mineTool;
    public bool IsPurchased => isPurchased;

    public bool CanPurchase()
    {
        if (mineTool == null)
            return false;

        if (purchaseOnlyOnce && isPurchased)
            return false;

        return true;
    }

    public void Purchase()
    {
        if (!CanPurchase())
            return;

        isPurchased = true;

        ApplyToolToTargets();
        ApplyToggleObjects();

        onPurchased?.Invoke();
    }

    private void ApplyToolToTargets()
    {
        if (!equipImmediately)
            return;

        if (targetMines == null || targetMines.Length == 0)
            return;

        for (int i = 0; i < targetMines.Length; i++)
        {
            Mine mine = targetMines[i];
            if (mine == null) continue;

            mine.SetTool(mineTool);
        }
    }

    private void ApplyToggleObjects()
    {
        SetActiveAll(activateOnPurchase, true);
        SetActiveAll(deactivateOnPurchase, false);
    }

    private void SetActiveAll(GameObject[] targets, bool value)
    {
        if (targets == null) return;

        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i] == null) continue;
            targets[i].SetActive(value);
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (targetMines == null || targetMines.Length > 0)
            return;

        Mine mine = GetComponent<Mine>();
        if (mine != null)
        {
            targetMines = new Mine[] { mine };
        }
    }
#endif
}