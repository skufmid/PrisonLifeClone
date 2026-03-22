using System.Collections.Generic;
using UnityEngine;

public class Jail : MonoBehaviour
{
    [SerializeField] private int maxCapacity = 4;
    [SerializeField] private Transform entryPoint;

    private readonly HashSet<Prisoner> prisoners = new();

    public bool CanAcceptPrisoner => prisoners.Count < maxCapacity;
    public Transform EntryPoint => entryPoint != null ? entryPoint : transform;

    public bool TryAccept(Prisoner prisoner)
    {
        if (prisoner == null) return false;
        if (!CanAcceptPrisoner) return false;
        if (prisoners.Contains(prisoner)) return false;

        prisoners.Add(prisoner);
        return true;
    }

    public void Remove(Prisoner prisoner)
    {
        if (prisoner == null) return;
        prisoners.Remove(prisoner);
    }
}