using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
public class Mine : ActionBase<Rock>
{
    [SerializeField] private MineTool defaultTool;

    [Header("Tool Mount Points")]
    [SerializeField] private Transform oneHandSocket;
    [SerializeField] private Transform twoHandSocket;
    [SerializeField] private Transform rideSocket;

    [Header("Output")]
    [SerializeField] private bool IsMiner;
    private TileStack inputTileStack;

    private MineTool currentTool;
    public MineTool CurrentTool => currentTool;

    private string startAnimName;
    private string stopAnimName;
    private Animator anim;
    private AudioSource source;
    private GameObject activeToolObject;
    private MineTool activeToolPrefabSource;

    private const int ROCK_LAYER = 1 << 6;

    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();

        if (currentTool == null)
            SetTool(defaultTool, false);

        if (IsMiner)
        {
            inputTileStack =
                FindAnyObjectByType<HandcuffMachine>()
                .GetComponentInChildren<DepositTile>()
                .GetComponent<TileStack>();
        }
    }

    public bool SetTool(MineTool newTool, bool stopCurrentAction = true)
    {
        if (newTool == null) return false;

        if (stopCurrentAction && IsActing)
            StopAction();

        currentTool = newTool;
        UpdateAnimNames();
        RefreshToolObject();
        return true;
    }

    public void SetOutputTileStack(TileStack tileStack)
    {
        inputTileStack = tileStack;
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
            if (target == null || currentTool == null)
            {
                StopAction();
                yield break;
            }

            yield return new WaitForSeconds(currentTool.interval);
            MineOnce();
        }
    }

    private void MineOnce()
    {
        if (currentTool == null)
        {
            StopAction();
            return;
        }

        SFXManager.instance.Play(source, SFXType.Mine);

        Vector3 center = transform.position + transform.forward * currentTool.range * 0.5f;
        Collider[] hits = Physics.OverlapSphere(center, currentTool.range, ROCK_LAYER);

        int minedCount = 0;

        foreach (Collider hit in hits)
        {
            Rock rock = hit.GetComponent<Rock>();
            if (rock == null) continue;

            bool mined = TryMineRock(rock);
            if (!mined) continue;

            minedCount++;

            if (minedCount >= currentTool.maxMineAmount)
                break;
        }

        if (minedCount == 0)
            StopAction();
    }

    private bool TryMineRock(Rock rock)
    {
        if (rock == null) return false;


        // 지정된 TileStack이 있으면 거기로 바로 보냄
        if (inputTileStack != null)
            return rock.TryMineToTileStack(inputTileStack);

        // 지정된 TileStack이 없으면 기존 방식 그대로 Carry로 감
        rock.Mine(owner);
        return true;
    }

    private void RefreshToolObject()
    {
        if (activeToolObject == null) return;

        if (currentTool == null || currentTool.toolPrefab == null)
        {
            Destroy(activeToolObject);
            activeToolObject = null;
            activeToolPrefabSource = null;
            return;
        }

        if (activeToolPrefabSource != currentTool)
        {
            bool wasActive = activeToolObject.activeSelf;

            Destroy(activeToolObject);
            activeToolObject = Instantiate(currentTool.toolPrefab);
            activeToolPrefabSource = currentTool;

            ApplyToolTransform();
            activeToolObject.SetActive(wasActive);
            return;
        }

        ApplyToolTransform();
    }

    private void EnableToolObject(bool enable)
    {
        if (currentTool == null || currentTool.toolPrefab == null) return;

        if (activeToolObject == null || activeToolPrefabSource != currentTool)
        {
            if (activeToolObject != null)
                Destroy(activeToolObject);

            activeToolObject = Instantiate(currentTool.toolPrefab);
            activeToolPrefabSource = currentTool;
        }

        ApplyToolTransform();
        activeToolObject.SetActive(enable);
    }

    private void ApplyToolTransform()
    {
        if (activeToolObject == null || currentTool == null) return;

        Transform mount = GetMountPoint(currentTool.equipType);
        activeToolObject.transform.SetParent(mount, false);
        activeToolObject.transform.localPosition = currentTool.positionOffset;
        activeToolObject.transform.localRotation = Quaternion.identity;
        activeToolObject.transform.localScale = Vector3.one;
    }

    private Transform GetMountPoint(MineToolEquipType type)
    {
        return type switch
        {
            MineToolEquipType.OneHand => oneHandSocket != null ? oneHandSocket : transform,
            MineToolEquipType.TwoHand => twoHandSocket != null ? twoHandSocket : transform,
            MineToolEquipType.Ride => rideSocket != null ? rideSocket : transform,
            _ => transform
        };
    }
}