using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Miner : CharacterBase
{
    [Header("Move")]
    private float stopDistance = 0.08f;
    private float mineDistance = 0.3f;
    private float stayDurationAtPoint = 1f;

    private Transform startPoint;
    private Transform endPoint;
    private TileStack outputTileStack;

    private Vector3 targetPosition;
    private bool hasTargetPosition;

    private bool isMovingToEnd = true;
    private float stayTimer;

    protected override void Update()
    {
        UpdateRoute();
        UpdateMovementInput();
        base.Update();
    }

    public void InitializeLane(Transform laneStartPoint, Transform laneEndPoint, TileStack targetTileStack)
    {
        startPoint = laneStartPoint;
        endPoint = laneEndPoint;
        outputTileStack = targetTileStack;

        isMovingToEnd = true;
        stayTimer = 0f;

        SetTarget(endPoint != null ? endPoint.position : transform.position);
    }

    private void UpdateRoute()
    {
        if (startPoint == null || endPoint == null)
        {
            hasTargetPosition = false;
            stayTimer = 0f;
            SetInput(Vector2.zero);
            return;
        }

        if (stayTimer > 0f)
        {
            stayTimer -= Time.deltaTime;

            if (stayTimer <= 0f)
            {
                isMovingToEnd = !isMovingToEnd;
                SetTarget(isMovingToEnd ? endPoint.position : startPoint.position);
            }

            return;
        }

        if (!hasTargetPosition) return;
        if (!IsCloseEnough(targetPosition)) return;

        hasTargetPosition = false;
        SetInput(Vector2.zero);

        if (isMovingToEnd)
        {
            TryMineNearestRockToOutput();
        }

        stayTimer = stayDurationAtPoint;
    }

    private void UpdateMovementInput()
    {
        if (stayTimer > 0f)
        {
            SetInput(Vector2.zero);
            return;
        }

        if (!hasTargetPosition)
        {
            SetInput(Vector2.zero);
            return;
        }

        Vector3 delta = targetPosition - transform.position;
        delta.y = 0f;

        if (delta.sqrMagnitude <= stopDistance * stopDistance)
        {
            SetInput(Vector2.zero);
            return;
        }

        Vector3 dir = delta.normalized;
        SetInput(new Vector2(dir.x, dir.z));
    }

    private void TryMineNearestRockToOutput()
    {
        if (outputTileStack == null) return;
        if (outputTileStack.IsFull) return;

        Rock targetRock = FindNearestMineableRock();
        if (targetRock == null) return;

        targetRock.TryMineToTileStack(outputTileStack);
    }

    private Rock FindNearestMineableRock()
    {
        Rock[] rocks = FindObjectsByType<Rock>(FindObjectsSortMode.None);

        Rock nearestRock = null;
        float nearestSqrDistance = float.MaxValue;
        float maxSqrDistance = mineDistance * mineDistance;

        for (int i = 0; i < rocks.Length; i++)
        {
            Rock rock = rocks[i];
            if (rock == null) continue;
            if (!rock.gameObject.activeInHierarchy) continue;

            Vector3 delta = rock.transform.position - transform.position;
            delta.y = 0f;

            float sqrDistance = delta.sqrMagnitude;
            if (sqrDistance > maxSqrDistance) continue;

            if (sqrDistance < nearestSqrDistance)
            {
                nearestSqrDistance = sqrDistance;
                nearestRock = rock;
            }
        }

        return nearestRock;
    }

    private void SetTarget(Vector3 position)
    {
        targetPosition = position;
        hasTargetPosition = true;
    }

    private bool IsCloseEnough(Vector3 destination)
    {
        Vector3 delta = destination - transform.position;
        delta.y = 0f;
        return delta.sqrMagnitude <= stopDistance * stopDistance;
    }
}