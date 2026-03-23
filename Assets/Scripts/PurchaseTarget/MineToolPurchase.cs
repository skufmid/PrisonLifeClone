using UnityEngine;

public class MineToolPurchase : PurchaseTargetBase
{
    [SerializeField] private MineTool mineTool;
    [SerializeField] private Mine[] targetMines;

    protected override void ApplyPurchase()
    {
        if (mineTool == null) return;

        if (targetMines == null) return;

        for (int i = 0; i < targetMines.Length; i++)
        {
            Mine mine = targetMines[i];
            if (mine == null) continue;

            mine.SetTool(mineTool);
        }
    }
}