using UnityEngine;

public class JailEntryQueue : PrisonerQueue
{
    [SerializeField] private JailQueue jailQueue;

    private void Update()
    {
        TrySendFrontPrisoner();
    }

    private void TrySendFrontPrisoner()
    {
        if (jailQueue == null) return;

        Prisoner frontPrisoner = GetFrontPrisoner();
        if (frontPrisoner == null) return;
        if (!frontPrisoner.IsAtTarget) return;
        if (!jailQueue.CanEnqueue(frontPrisoner)) return;

        Prisoner prisoner = PopFront();
        if (prisoner == null) return;

        jailQueue.Enqueue(prisoner);
    }
}