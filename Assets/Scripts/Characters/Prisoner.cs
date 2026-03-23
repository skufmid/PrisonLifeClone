using UnityEngine;

public class Prisoner : CharacterBase
{
    private enum PrisonerState
    {
        Queueing,
        InJail
    }

    [Header("Queue Move")]
    [SerializeField] private float stopDistance = 0.08f;

    [Header("Handcuff")]
    [SerializeField] private int minRequiredHandcuff = 2;
    [SerializeField] private int maxRequiredHandcuff = 4;

    private PrisonerQueue currentQueue;
    private JailQueue currentJailQueue;
    private int queueIndex = -1;
    private Vector3 targetPosition;
    private bool hasTargetPosition;

    private PrisonerState state = PrisonerState.Queueing;
    private int requiredHandcuff;
    private int currentHandcuff;

    public int QueueIndex => queueIndex;
    public bool IsAtTarget => !hasTargetPosition || IsCloseEnough(targetPosition);

    public int RequiredHandcuff => requiredHandcuff;
    public int CurrentHandcuff => currentHandcuff;
    public int RemainingHandcuff => Mathf.Max(0, requiredHandcuff - currentHandcuff);

    public bool IsReadyForJailEntry => currentHandcuff >= requiredHandcuff;
    public bool IsInJail => state == PrisonerState.InJail;

    protected virtual void Start()
    {
        requiredHandcuff = Random.Range(minRequiredHandcuff, maxRequiredHandcuff + 1);
        currentHandcuff = 0;
    }

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

        if (state != PrisonerState.InJail)
            state = PrisonerState.Queueing;
    }

    public void ClearQueueSlot(PrisonerQueue queue)
    {
        if (currentQueue != queue) return;

        currentQueue = null;
        queueIndex = -1;
        hasTargetPosition = false;
        SetInput(Vector2.zero);
    }

    public bool TryReceiveHandcuff()
    {
        if (IsReadyForJailEntry) return false;
        if (state == PrisonerState.InJail) return false;

        currentHandcuff++;
        return true;
    }

    public void EnterJailQueue(JailQueue jailQueue)
    {
        currentJailQueue = jailQueue;
        state = PrisonerState.InJail;
    }

    private void OnDisable()
    {
        LeaveQueue();
    }
}