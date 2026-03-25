using UnityEngine;
using UnityEngine.Events;

public class JailQueue : PrisonerQueue
{
    [SerializeField] private int maxCapacity = 20;

    public int MaxCapacity => maxCapacity;
    public bool HasSpace => Count < maxCapacity;

    bool isFirstMaxCapacityReached = true;
    [SerializeField] private UnityEvent onMaxCapacityReached;
    [SerializeField] private UnityEvent onPrisonerEnqueued;

    public override bool CanEnqueue(Prisoner prisoner)
    {
        if (!HasSpace)
        {
            if (isFirstMaxCapacityReached)
            {
                isFirstMaxCapacityReached = false;
                onMaxCapacityReached?.Invoke();
            }
            return false;
        }

        return base.CanEnqueue(prisoner);
    }

    public override bool Enqueue(Prisoner prisoner)
    {
        if (!CanEnqueue(prisoner)) return false;

        bool success = base.Enqueue(prisoner);
        if (!success) return false;

        onPrisonerEnqueued?.Invoke();
        prisoner.EnterJailQueue(this);
        return true;
    }

    public void ExtendCapacity(int amount)
    {
        maxCapacity += amount;
    }
}