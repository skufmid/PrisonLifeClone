using System.Collections.Generic;
using UnityEngine;

public class PrisonerQueue : MonoBehaviour
{
    [Header("Queue Points (Front -> Back)")]
    [SerializeField] private Transform[] queuePoints;

    [Header("Overflow")]
    [SerializeField] private float overflowSpacing = 1.2f;

    private readonly List<Prisoner> prisoners = new();

    public int Count => prisoners.Count;

    public void Enqueue(Prisoner prisoner)
    {
        if (prisoner == null) return;
        if (prisoners.Contains(prisoner)) return;

        prisoners.Add(prisoner);
        RefreshQueueSlots();
    }

    public void Remove(Prisoner prisoner)
    {
        if (prisoner == null) return;

        if (!prisoners.Remove(prisoner)) return;

        RefreshQueueSlots();
    }

    public bool IsFront(Prisoner prisoner)
    {
        if (prisoner == null) return false;
        if (prisoners.Count == 0) return false;

        return prisoners[0] == prisoner;
    }

    public Prisoner GetFrontPrisoner()
    {
        if (prisoners.Count == 0) return null;
        return prisoners[0];
    }

    public void PopFront()
    {
        if (prisoners.Count == 0) return;

        Prisoner front = prisoners[0];
        prisoners.RemoveAt(0);

        if (front != null)
            front.ClearQueueSlot(this);

        RefreshQueueSlots();
    }

    private void RefreshQueueSlots()
    {
        for (int i = 0; i < prisoners.Count; i++)
        {
            Prisoner prisoner = prisoners[i];
            if (prisoner == null) continue;

            prisoner.SetQueueSlot(this, i, GetQueuePosition(i));
        }
    }

    private Vector3 GetQueuePosition(int index)
    {
        if (queuePoints == null || queuePoints.Length == 0)
            return transform.position;

        if (index < queuePoints.Length)
            return queuePoints[index].position;

        Transform lastPoint = queuePoints[queuePoints.Length - 1];
        Vector3 extendDir = GetOverflowDirection();

        int overflowIndex = index - queuePoints.Length + 1;
        return lastPoint.position + extendDir * overflowSpacing * overflowIndex;
    }

    private Vector3 GetOverflowDirection()
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

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (queuePoints == null || queuePoints.Length == 0) return;

        Gizmos.color = Color.cyan;

        for (int i = 0; i < queuePoints.Length; i++)
        {
            if (queuePoints[i] == null) continue;

            Gizmos.DrawWireSphere(queuePoints[i].position, 0.15f);

            if (i < queuePoints.Length - 1 && queuePoints[i + 1] != null)
            {
                Gizmos.DrawLine(queuePoints[i].position, queuePoints[i + 1].position);
            }
        }
    }
#endif
}