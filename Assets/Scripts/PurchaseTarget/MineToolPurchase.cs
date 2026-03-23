using UnityEngine;

public class MineToolPurchase : PurchaseTargetBase
{
    [Header("Mine Tool")]
    [SerializeField] private MineTool mineTool;

    [Header("Target Mines")]
    [SerializeField] private Mine[] targetMines;

    [Header("Options")]
    [SerializeField] private bool equipImmediately = true;

    protected override void ApplyPurchase()
    {
        if (mineTool == null) return;

        if (!equipImmediately) return;
        if (targetMines == null) return;

        for (int i = 0; i < targetMines.Length; i++)
        {
            Mine mine = targetMines[i];
            if (mine == null) continue;

            mine.SetTool(mineTool);
        }
    }
}