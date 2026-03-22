using UnityEngine;

public class Prisoner : CharacterBase
{
    private enum PrisonerState
    {
        Queueing,
        EnteringPrison
    }

    [Header("Queue Move")]
    [SerializeField] private float stopDistance = 0.08f;

    [Header("Handcuff")]
    [SerializeField] private int minRequiredHandcuff = 2;
    [SerializeField] private int maxRequiredHandcuff = 4;

    [Header("Prison Enter")]
    [SerializeField] private Collider[] collidersToDisableWhileEntering;

    private PrisonerQueue currentQueue;
    private int queueIndex = -1;
    private Vector3 targetPosition;
    private bool hasTargetPosition;

    private PrisonerState state = PrisonerState.Queueing;
    private int requiredHandcuff;
    private int currentHandcuff;

    public int QueueIndex => queueIndex;
    public bool IsAtTarget => !hasTargetPosition || IsCloseEnough(targetPosition);
    public bool IsFrontPrisoner => currentQueue != null && currentQueue.IsFront(this);

    public int RequiredHandcuff => requiredHandcuff;
    public int CurrentHandcuff => currentHandcuff;
    public int RemainingHandcuff => Mathf.Max(0, requiredHandcuff - currentHandcuff);
    public bool IsReadyToEnter => currentHandcuff >= requiredHandcuff;
    public bool IsEnteringPrison => state == PrisonerState.EnteringPrison;

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

            if (state == PrisonerState.EnteringPrison)
            {
                FinishEnterPrison();
            }

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
        if (state == PrisonerState.EnteringPrison) return;

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
        if (IsReadyToEnter) return false;

        currentHandcuff++;

        // 필요하면 여기서 수갑 착용 애니메이션/이펙트 추가
        // animator.SetTrigger("WearHandcuff");

        return true;
    }

    public void StartEnterPrison()
    {
        if (!IsReadyToEnter) return;
        if (state == PrisonerState.EnteringPrison) return;
        if (GameManager.Instance.PrisonPoint == null) return;

        if (currentQueue != null)
        {
            PrisonerQueue oldQueue = currentQueue;
            currentQueue = null;
            queueIndex = -1;
            oldQueue.Remove(this);
        }

        state = PrisonerState.EnteringPrison;
        targetPosition = GameManager.Instance.PrisonPoint.position;
        hasTargetPosition = true;

        SetBodyCollidersEnabled(false);

        // 필요하면 여기서 입장 애니메이션
        // animator.SetBool("IsHandcuffed", true);
    }

    private void FinishEnterPrison()
    {
        hasTargetPosition = false;
        SetInput(Vector2.zero);
        SetBodyCollidersEnabled(true);

        // 필요하면 도착 후 상태 처리
        // animator.SetBool("IsHandcuffed", true);
    }

    private void SetBodyCollidersEnabled(bool enabled)
    {
        if (collidersToDisableWhileEntering == null) return;

        for (int i = 0; i < collidersToDisableWhileEntering.Length; i++)
        {
            Collider col = collidersToDisableWhileEntering[i];
            if (col == null) continue;

            // CharacterController가 Collider 계열인 경우 이동이 끊길 수 있으니 제외
            if (col is CharacterController) continue;

            col.enabled = enabled;
        }
    }

    private void OnDisable()
    {
        LeaveQueue();
    }
}