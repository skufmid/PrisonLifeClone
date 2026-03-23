using System.Collections;
using UnityEngine;

public class Prisoner : CharacterBase
{
    private enum PrisonerState
    {
        Queueing,
        EnteringJail,
        InJail
    }

    [Header("Queue Move")]
    [SerializeField] private float stopDistance = 0.08f;

    [Header("Handcuff")]
    [SerializeField] private int minRequiredHandcuff = 2;
    [SerializeField] private int maxRequiredHandcuff = 4;

    private PrisonerQueue currentQueue;
    private int queueIndex = -1;
    private Vector3 targetPosition;
    private bool hasTargetPosition;

    private PrisonerState state = PrisonerState.Queueing;

    private int requiredHandcuff;
    private int currentHandcuff;
    private Jail currentJail;

    public int QueueIndex => queueIndex;
    public bool IsAtTarget => !hasTargetPosition || IsCloseEnough(targetPosition);
    public bool IsFrontPrisoner => currentQueue != null && currentQueue.IsFront(this);

    public int RequiredHandcuff => requiredHandcuff;
    public int CurrentHandcuff => currentHandcuff;
    public int RemainingHandcuff => Mathf.Max(0, requiredHandcuff - currentHandcuff);
    public bool IsReadyForJailQueue => currentHandcuff >= requiredHandcuff;
    public bool IsEnteringJail => state == PrisonerState.EnteringJail;

    protected virtual void Start()
    {
        requiredHandcuff = Random.Range(minRequiredHandcuff, maxRequiredHandcuff + 1);
        currentHandcuff = 0;

        Debug.Log("detectCollisions: " + controller.detectCollisions);

        Collider[] cols = GetComponentsInChildren<Collider>();
        for (int i = 0; i < cols.Length; i++)
        {
            Debug.Log(cols[i].name + " / enabled: " + cols[i].enabled + " / type: " + cols[i].GetType().Name);
        }
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

            if (state == PrisonerState.EnteringJail)
                FinishEnterJail();

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
        if (state == PrisonerState.EnteringJail || state == PrisonerState.InJail)
            return;

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

    public bool TryReceiveHandcuff()
    {
        if (state != PrisonerState.Queueing) return false;
        if (IsReadyForJailQueue) return false;

        currentHandcuff++;
        return true;
    }

    public void EnterJail(Jail jail)
    {
        if (jail == null) return;
        if (state == PrisonerState.EnteringJail || state == PrisonerState.InJail) return;

        if (currentQueue != null)
        {
            PrisonerQueue oldQueue = currentQueue;
            currentQueue = null;
            queueIndex = -1;
            oldQueue.Remove(this);
        }

        currentJail = jail;
        state = PrisonerState.EnteringJail;
        targetPosition = jail.EntryPoint.position;
        hasTargetPosition = true;
    }

    private void FinishEnterJail()
    {
        hasTargetPosition = false;
        SetInput(Vector2.zero);
        state = PrisonerState.InJail;
    }

    public void SetCollisionEnable(bool enable)
    {
        controller.enabled = enable;
    }

    private void OnDisable()
    {
        LeaveQueue();
    }
}