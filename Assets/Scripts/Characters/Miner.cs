using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Miner : CharacterBase
{
    [Header("Move")]
    [SerializeField] private float stopDistance = 0.08f;

    private Transform startPoint;
    private Transform endPoint;

    private Vector3 targetPosition;
    private bool hasTargetPosition;

    private bool goingToEnd = true;

    protected override void Update()
    {
        UpdateMovement();
        base.Update();
    }

    public void InitializeLane(Transform laneStartPoint, Transform laneEndPoint)
    {
        startPoint = laneStartPoint;
        endPoint = laneEndPoint;

        goingToEnd = true;

        if (endPoint == null)
        {
            hasTargetPosition = false;
            SetInput(Vector2.zero);
            return;
        }

        SetTarget(endPoint.position);
    }

    private void UpdateMovement()
    {
        if (!hasTargetPosition)
        {
            SetInput(Vector2.zero);
            return;
        }

        Vector3 delta = targetPosition - transform.position;
        delta.y = 0f;

        if (delta.sqrMagnitude <= stopDistance * stopDistance)
        {
            OnArrived();
            return;
        }

        Vector3 dir = delta.normalized;
        SetInput(new Vector2(dir.x, dir.z));
    }

    private void OnArrived()
    {
        // 방향 전환
        goingToEnd = !goingToEnd;

        Transform nextTarget = goingToEnd ? endPoint : startPoint;

        if (nextTarget == null)
        {
            hasTargetPosition = false;
            SetInput(Vector2.zero);
            return;
        }

        SetTarget(nextTarget.position);
    }

    private void SetTarget(Vector3 position)
    {
        targetPosition = position;
        hasTargetPosition = true;
    }
}