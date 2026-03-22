using UnityEngine;

public class BuildingEntryQueue : PrisonerQueue
{
    [SerializeField] private JailEntryQueue jailEntryQueue;

    private void Update()
    {
        TryMoveFrontPrisonerToJailQueue();
    }

    private void TryMoveFrontPrisonerToJailQueue()
    {
        if (jailEntryQueue == null) return;

        Prisoner frontPrisoner = GetFrontPrisoner();
        if (frontPrisoner == null) return;
        if (!frontPrisoner.IsAtTarget) return;
        if (!frontPrisoner.IsReadyForJailQueue) return;

        Prisoner prisoner = PopFront();
        if (prisoner == null) return;

        jailEntryQueue.Enqueue(prisoner);
    }
}