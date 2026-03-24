using System;
using UnityEngine;

public class MinerPurchase : PurchaseTargetBase
{
    [Serializable]
    private class MinerLane
    {
        public Transform startPoint;
        public Transform endPoint;
    }

    [Header("Miner")]
    [SerializeField] private Miner minerPrefab;
    [SerializeField] private int hireCount = 3;

    [Header("Lane")]
    [SerializeField] private MinerLane[] lanes;

    protected override void ApplyPurchase()
    {
        if (minerPrefab == null)
        {
            Debug.LogWarning($"{name}: minerPrefab이 비어 있습니다.");
            return;
        }

        if (lanes == null || lanes.Length == 0)
        {
            Debug.LogWarning($"{name}: lanes가 비어 있습니다.");
            return;
        }

        int spawnCount = Mathf.Min(hireCount, lanes.Length);

        for (int i = 0; i < spawnCount; i++)
        {
            MinerLane lane = lanes[i];
            if (lane == null) continue;

            if (lane.startPoint == null || lane.endPoint == null)
            {
                Debug.LogWarning($"{name}: {i}번 lane의 startPoint 또는 endPoint가 비어 있습니다.");
                continue;
            }

            Miner miner = Instantiate(
                minerPrefab,
                lane.startPoint.position,
                lane.startPoint.rotation
            );

            miner.InitializeLane(lane.startPoint, lane.endPoint);
        }
    }
}