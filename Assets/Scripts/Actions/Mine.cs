using System.Collections;
using UnityEngine;

public class Mine : ActionBase<Rock>
{
    [SerializeField] private MineTool defaultTool;

    [Header("Tool Mount Points")]
    [SerializeField] private Transform oneHandSocket;
    [SerializeField] private Transform twoHandSocket;
    [SerializeField] private Transform rideSocket;

    private MineTool currentTool;
    public MineTool CurrentTool
    {
        get => currentTool;
        private set
        {
            currentTool = value;
            UpdateAnimNames();
        }
    }

    private string startAnimName;
    private string stopAnimName;

    private Animator anim;
    private GameObject activeToolObject;
    private MineTool activeToolPrefabSource;

    private const int ROCK_LAYER = 1 << 6;

    protected override void Awake()
    {
        base.Awake();

        anim = GetComponent<Animator>();

        if (CurrentTool == null)
            CurrentTool = defaultTool;
    }

    private void UpdateAnimNames()
    {
        if (currentTool == null)
        {
            startAnimName = string.Empty;
            stopAnimName = string.Empty;
            return;
        }

        startAnimName = "Start" + currentTool.toolName;
        stopAnimName = "Stop" + currentTool.toolName;
    }

    protected override void OnStarted()
    {
        EnableToolObject(true);

        if (anim != null && !string.IsNullOrEmpty(startAnimName))
            anim.SetTrigger(startAnimName);
    }

    protected override void OnStopped()
    {
        if (anim != null && !string.IsNullOrEmpty(stopAnimName))
            anim.SetTrigger(stopAnimName);

        EnableToolObject(false);
    }

    protected override IEnumerator CoAction()
    {
        while (true)
        {
            if (target == null)
            {
                StopAction();
                yield break;
            }

            yield return new WaitForSeconds(CurrentTool.interval);
            MineOnce();
        }
    }

    private void MineOnce()
    {
        Vector3 center = transform.position + transform.forward * CurrentTool.range * 0.5f;
        Collider[] hits = Physics.OverlapSphere(center, CurrentTool.range, ROCK_LAYER);

        int minedCount = 0;

        foreach (var hit in hits)
        {
            Rock rock = hit.GetComponent<Rock>();
            if (rock == null) continue;

            rock.Mine(owner);
            minedCount++;

            if (minedCount >= CurrentTool.maxMineAmount)
                break;
        }

        if (minedCount == 0) StopAction();
    }

    private void EnableToolObject(bool enable)
    {
        if (CurrentTool == null || CurrentTool.toolPrefab == null)
            return;

        if (activeToolObject == null || activeToolPrefabSource != CurrentTool)
        {
            if (activeToolObject != null)
                Destroy(activeToolObject);

            activeToolObject = Instantiate(CurrentTool.toolPrefab);
            activeToolPrefabSource = CurrentTool;
        }

        Transform mount = GetMountPoint(CurrentTool.equipType);

        activeToolObject.transform.SetParent(mount, false);
        activeToolObject.transform.localPosition = Vector3.zero;
        activeToolObject.transform.localRotation = Quaternion.identity;
        activeToolObject.transform.localScale = Vector3.one;

        activeToolObject.SetActive(enable);
    }

    private Transform GetMountPoint(MineToolEquipType type)
    {
        return type switch
        {
            MineToolEquipType.OneHand => oneHandSocket != null ? oneHandSocket : transform,
            MineToolEquipType.TwoHand => twoHandSocket != null ? twoHandSocket : transform,
            MineToolEquipType.Ride => rideSocket != null ? rideSocket : transform,
            _ => transform,
        };
    }
}