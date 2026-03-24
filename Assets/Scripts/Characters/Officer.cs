using UnityEngine;

[RequireComponent(typeof(Carry))]
public class Officer : CharacterBase
{
    private enum OfficerState
    {
        MovingToA,
        LoadingAtA,
        MovingToB,
        UnloadingAtB
    }

    [Header("Route")]
    [SerializeField] private TileStack pointAStack;
    [SerializeField] private TileStack pointBStack;
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    [Header("Move")]
    [SerializeField] private float stopDistance = 0.1f;

    private Carry carry;
    private OfficerState state = OfficerState.MovingToA;

    protected override void Awake()
    {
        base.Awake();
        carry = GetComponent<Carry>();
    }

    protected override void Update()
    {
        UpdateOfficer();
        base.Update();
    }

    public void Setup(TileStack sourceStack, TileStack destinationStack)
    {
        pointAStack = sourceStack;
        pointBStack = destinationStack;
        state = OfficerState.MovingToA;
    }

    private void UpdateOfficer()
    {
        if (carry == null)
        {
            SetInput(Vector2.zero);
            return;
        }

        switch (state)
        {
            case OfficerState.MovingToA:
                MoveToPoint(GetPointAPosition());

                if (IsAtPosition(GetPointAPosition()))
                {
                    state = OfficerState.LoadingAtA;
                    SetInput(Vector2.zero);
                }
                break;

            case OfficerState.LoadingAtA:
                SetInput(Vector2.zero);
                TryLoadHandcuffsAtA();
                break;

            case OfficerState.MovingToB:
                MoveToPoint(GetPointBPosition());

                if (IsAtPosition(GetPointBPosition()))
                {
                    state = OfficerState.UnloadingAtB;
                    SetInput(Vector2.zero);
                }
                break;

            case OfficerState.UnloadingAtB:
                SetInput(Vector2.zero);
                TryUnloadHandcuffsAtB();
                break;
        }
    }

    private void TryLoadHandcuffsAtA()
    {
        if (pointAStack == null)
            return;

        bool loadedAny = false;

        while (pointAStack.TryTakeLastOfType(CarryItemType.Handcuff, out CarriableBase item))
        {
            if (item == null)
                continue;

            if (carry.TryAdd(item))
            {
                loadedAny = true;
                continue;
            }

            pointAStack.TryAdd(item);
            state = OfficerState.MovingToB;
            return;
        }

        if (loadedAny || HasHandcuffInCarry())
        {
            state = OfficerState.MovingToB;
        }
    }

    private void TryUnloadHandcuffsAtB()
    {
        if (pointBStack == null)
            return;

        while (carry.TryTakeLastOfTypeFromAny(CarryItemType.Handcuff, out CarriableBase item))
        {
            if (item == null)
                continue;

            if (pointBStack.TryAdd(item))
                continue;

            carry.TryAdd(item);
            return;
        }

        state = OfficerState.MovingToA;
    }

    private bool HasHandcuffInCarry()
    {
        return carry.HasItem(CarryItemType.Handcuff);
    }

    private void MoveToPoint(Vector3 destination)
    {
        Vector3 delta = destination - transform.position;
        delta.y = 0f;

        if (delta.sqrMagnitude <= stopDistance * stopDistance)
        {
            SetInput(Vector2.zero);
            return;
        }

        Vector3 dir = delta.normalized;
        SetInput(new Vector2(dir.x, dir.z));
    }

    private bool IsAtPosition(Vector3 destination)
    {
        Vector3 delta = destination - transform.position;
        delta.y = 0f;
        return delta.sqrMagnitude <= stopDistance * stopDistance;
    }

    private Vector3 GetPointAPosition()
    {
        if (pointA != null) return pointA.position;
        if (pointAStack != null) return pointAStack.transform.position;
        return transform.position;
    }

    private Vector3 GetPointBPosition()
    {
        if (pointB != null) return pointB.position;
        if (pointBStack != null) return pointBStack.transform.position;
        return transform.position;
    }
}