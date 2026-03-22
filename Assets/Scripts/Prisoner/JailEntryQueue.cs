using UnityEngine;

public class JailEntryQueue : PrisonerQueue
{
    [SerializeField] private Jail jail;

    private void Update()
    {
        TryEnterFrontPrisoner();
    }

    private void TryEnterFrontPrisoner()
    {
        if (jail == null) return;
        if (!jail.CanAcceptPrisoner) return;

        Prisoner frontPrisoner = GetFrontPrisoner();
        if (frontPrisoner == null) return;
        if (!frontPrisoner.IsAtTarget) return;

        if (!jail.TryAccept(frontPrisoner)) return;

        Prisoner prisoner = PopFront();
        if (prisoner == null) return;

        prisoner.EnterJail(jail);
    }
}