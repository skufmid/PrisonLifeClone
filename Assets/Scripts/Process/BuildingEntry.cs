using System.Collections;
using UnityEngine;

public class BuildingEntry : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TileStack inputStack;
    [SerializeField] private TileStack outputStack;
    [SerializeField] private BuildingEntryQueue prisonerQueue;

    [Header("Loop")]
    [SerializeField] private float giveInterval = 0.05f;
    [SerializeField] private float nextPrisonerInterval = 1f;

    [Header("Type")]
    [SerializeField] private CarryItemType requiredType = CarryItemType.Handcuff;

    private Coroutine coLoop;
    private WaitForSeconds giveIntervalWait;
    private WaitForSeconds nextPrisonerWait;

    private void Awake()
    {
        giveIntervalWait = new WaitForSeconds(giveInterval);
        nextPrisonerWait = new WaitForSeconds(nextPrisonerInterval);
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
                yield return nextPrisonerWait;
            else
                yield return giveIntervalWait;
        }
    }

    private bool TryProcessFrontPrisoner()
    {
        if (inputStack == null) return false;
        if (prisonerQueue == null) return false;

        Prisoner prisoner = prisonerQueue.GetFrontPrisoner();
        if (prisoner == null) return false;
        if (!prisoner.IsAtTarget) return false;
        if (!inputStack.HasItem(requiredType)) return false;

        if (!inputStack.TryTakeLast(out CarriableBase handcuffItem)) return false;
        if (handcuffItem == null) return false;

        bool received = prisoner.TryReceiveHandcuff();

        if (received)
        {
            Destroy(handcuffItem.gameObject);

            return true;
        }

        inputStack.TryAdd(handcuffItem);
        return false;
    }
}