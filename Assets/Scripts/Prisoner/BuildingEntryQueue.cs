using UnityEngine;

public class BuildingEntryQueue : PrisonerQueue
{
    [SerializeField] private JailEntryQueue jailEntryQueue;

    private void Update()
    {
        TrySendFrontPrisoner();
    }

    private void TrySendFrontPrisoner()
    {
        if (jailEntryQueue == null) return;

        Prisoner frontPrisoner = GetFrontPrisoner();
        if (frontPrisoner == null) return;
        if (!frontPrisoner.IsAtTarget) return;
        if (!frontPrisoner.IsReadyForJailEntry) return;
        if (!jailEntryQueue.CanEnqueue(frontPrisoner)) return;

        Prisoner prisoner = PopFront();
        if (prisoner == null) return;

        jailEntryQueue.Enqueue(prisoner);
    }
}