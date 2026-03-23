using UnityEngine;

public class JailQueue : PrisonerQueue
{
    [SerializeField] private int maxCapacity = 4;

    public int MaxCapacity => maxCapacity;
    public bool HasSpace => Count < maxCapacity;

    public override bool CanEnqueue(Prisoner prisoner)
    {
        if (!HasSpace) return false;
        return base.CanEnqueue(prisoner);
    }

    public override bool Enqueue(Prisoner prisoner)
    {
        if (!CanEnqueue(prisoner)) return false;

        bool success = base.Enqueue(prisoner);
        if (!success) return false;

        prisoner.EnterJailQueue(this);
        return true;
    }
}