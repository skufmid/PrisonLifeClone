using UnityEngine;

public class Prisoner : CharacterBase
{
    [Header("Queue Move")]
    [SerializeField] private float stopDistance = 0.08f;

    private PrisonerQueue currentQueue;
    private int queueIndex = -1;
    private Vector3 targetPosition;
    private bool hasTargetPosition;

    public int QueueIndex => queueIndex;
    public bool IsAtTarget => !hasTargetPosition || IsCloseEnough(targetPosition);
    public bool IsFrontPrisoner => currentQueue != null && currentQueue.IsFront(this);
    
    protected override void Update()
    {
        UpdateMovementInput();
        base.Update();
    }

    private void UpdateMovementInput()
    {
        if (!hasTargetPosition)
        {
            SetInput(Vector2.zero);
            return;
        }

        Vector3 delta = targetPosition - transform.position;
        delta.y = 0f;

        if (delta.sqrMagnitude <= stopDistance * stopDistance)
        {
            SetInput(Vector2.zero);
            return;
        }

        Vector3 dir = delta.normalized;
        SetInput(new Vector2(dir.x, dir.z));
    }

    private bool IsCloseEnough(Vector3 destination)
    {
        Vector3 delta = destination - transform.position;
        delta.y = 0f;
        return delta.sqrMagnitude <= stopDistance * stopDistance;
    }

    public void JoinQueue(PrisonerQueue queue)
    {
        if (queue == null) return;

        if (currentQueue == queue) return;

        LeaveQueue();
        queue.Enqueue(this);
    }

    public void LeaveQueue()
    {
        if (currentQueue == null) return;

        PrisonerQueue oldQueue = currentQueue;
        currentQueue = null;
        queueIndex = -1;
        hasTargetPosition = false;
        SetInput(Vector2.zero);

        oldQueue.Remove(this);
    }

    public void SetQueueSlot(PrisonerQueue queue, int index, Vector3 position)
    {
        currentQueue = queue;
        queueIndex = index;
        targetPosition = position;
        hasTargetPosition = true;
    }

    public void ClearQueueSlot(PrisonerQueue queue)
    {
        if (currentQueue != queue) return;

        currentQueue = null;
        queueIndex = -1;
        hasTargetPosition = false;
        SetInput(Vector2.zero);
    }

    private void OnDisable()
    {
        LeaveQueue();
    }
}