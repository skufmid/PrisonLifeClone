using System.Collections;
using UnityEngine;

public class BuildingEntry : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TileStack inputStack;
    [SerializeField] private PrisonerQueue prisonerQueue;

    [Header("Loop")]
    [SerializeField] private float giveInterval = 0.05f;
    [SerializeField] private float nextPrisonerInterval = 1.5f;

    [Header("Type")]
    [SerializeField] private CarryItemType requiredType = CarryItemType.Handcuff;

    private Coroutine coLoop;
    private WaitForSeconds GiveIntervalWait;
    private WaitForSeconds NextPrisonerWait;

    private void Awake()
    {
        GiveIntervalWait = new WaitForSeconds(giveInterval);
        NextPrisonerWait = new WaitForSeconds(nextPrisonerInterval);
    }

    private void OnEnable()
    {
        if (coLoop == null)
            coLoop = StartCoroutine(CoGiveLoop());
    }

    private void OnDisable()
    {
        if (coLoop != null)
        {
            StopCoroutine(coLoop);
            coLoop = null;
        }
    }

    private IEnumerator CoGiveLoop()
    {
        while (true)
        {
            if (TryProcessFrontPrisoner())
            {
                yield return NextPrisonerWait;
            }
            else
            {
                yield return GiveIntervalWait;
            }
        }
    }

    private bool TryProcessFrontPrisoner() // Prisoner가 감옥에 들어갔을 때 True 반환
    {
        if (inputStack == null) return false;
        if (prisonerQueue == null) return false;

        Prisoner prisoner = prisonerQueue.GetFrontPrisoner();
        if (prisoner == null) return false;
        if (prisoner.IsEnteringPrison) return false;
        if (!prisoner.IsAtTarget) return false;
        if (!inputStack.HasItem(requiredType)) return false;

        if (!inputStack.TryTakeLast(out CarriableBase handcuffItem)) return false;
        if (handcuffItem == null) return false;

        bool received = prisoner.TryReceiveHandcuff();

        if (received)
        {
            Destroy(handcuffItem.gameObject);

            if (prisoner.IsReadyToEnter)
            {
                prisoner.StartEnterPrison();
                return true;
            }
        }
        else
        {
            inputStack.TryAdd(handcuffItem);
        }
        return false;
    }
}