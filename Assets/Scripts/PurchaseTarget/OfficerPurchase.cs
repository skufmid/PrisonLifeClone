using UnityEngine;

public class OfficerPurchase : PurchaseTargetBase
{
    [Header("Officer Spawn")]
    [SerializeField] private Officer officerPrefab;
    [SerializeField] private Transform[] spawnPoints;

    [Header("Officer Route")]
    [SerializeField] private TileStack pointAStack;
    [SerializeField] private TileStack pointBStack;

    private int spawnedCount;

    public override bool CanPurchase()
    {
        if (!base.CanPurchase())
            return false;

        if (officerPrefab == null)
            return false;

        if (spawnPoints == null || spawnPoints.Length == 0)
            return false;

        if (spawnedCount >= spawnPoints.Length)
            return false;

        return true;
    }

    protected override void ApplyPurchase()
    {
        if (!CanPurchase())
            return;

        Transform spawnPoint = spawnPoints[spawnedCount];
        if (spawnPoint == null)
            return;

        Officer officer = Instantiate(
            officerPrefab,
            spawnPoint.position,
            spawnPoint.rotation);

        officer.Setup(pointAStack, pointBStack);

        spawnedCount++;
    }
}