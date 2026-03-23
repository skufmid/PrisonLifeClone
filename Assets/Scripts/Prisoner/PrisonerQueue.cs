using System.Collections.Generic;
using UnityEngine;

public abstract class PrisonerQueue : MonoBehaviour
{
    [Header("Queue Points (Front -> Back)")]
    [SerializeField] protected Transform[] queuePoints;

    [Header("Overflow")]
    [SerializeField] protected float overflowSpacing = 1.2f;

    protected readonly List<Prisoner> prisoners = new();

    public int Count => prisoners.Count;
    public bool IsEmpty => prisoners.Count == 0;

    public virtual bool CanEnqueue(Prisoner prisoner)
    {
        return prisoner != null && !prisoners.Contains(prisoner);
    }

    public virtual bool Enqueue(Prisoner prisoner)
    {
        if (!CanEnqueue(prisoner)) return false;

        prisoners.Add(prisoner);
        RefreshQueueSlots();
        return true;
    }

    public virtual bool Remove(Prisoner prisoner)
    {
        if (prisoner == null) return false;
        if (!prisoners.Remove(prisoner)) return false;

        prisoner.ClearQueueSlot(this);
        RefreshQueueSlots();
        return true;
    }

    public bool IsFront(Prisoner prisoner)
    {
        return prisoners.Count > 0 && prisoners[0] == prisoner;
    }

    public Prisoner GetFrontPrisoner()
    {
        if (prisoners.Count == 0) return null;
        return prisoners[0];
    }

    public Prisoner PopFront()
    {
        if (prisoners.Count == 0) return null;

        Prisoner front = prisoners[0];
        prisoners.RemoveAt(0);

        if (front != null)
            front.ClearQueueSlot(this);

        RefreshQueueSlots();
        return front;
    }

    protected virtual void RefreshQueueSlots()
    {
        for (int i = 0; i < prisoners.Count; i++)
        {
            Prisoner prisoner = prisoners[i];
            if (prisoner == null) continue;

            prisoner.SetQueueSlot(this, i, GetQueuePosition(i));
        }
    }

    protected virtual Vector3 GetQueuePosition(int index)
    {
        if (queuePoints == null || queuePoints.Length == 0)
            return transform.position;

        if (index < queuePoints.Length)
            return queuePoints[index].position;

        Transform lastPoint = queuePoints[queuePoints.Length - 1];
        Vector3 overflowDir = GetOverflowDirection();
        int overflowIndex = index - queuePoints.Length + 1;

        return lastPoint.position + overflowDir * overflowSpacing * overflowIndex;
    }

    protected virtual Vector3 GetOverflowDirection()
    {
        if (queuePoints == null || queuePoints.Length == 0)
            return -transform.forward;

        if (queuePoints.Length == 1)
            return -queuePoints[0].forward;

        Vector3 dir = queuePoints[queuePoints.Length - 1].position
                    - queuePoints[queuePoints.Length - 2].position;

        if (dir.sqrMagnitude <= 0.0001f)
            return -queuePoints[queuePoints.Length - 1].forward;

        return dir.normalized;
    }
}