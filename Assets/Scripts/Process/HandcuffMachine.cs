using UnityEngine;

public class HandcuffMachine : ProcessorBase
{
    [Header("Input / Output")]
    [SerializeField] private CarryItemType inputType = CarryItemType.Rock;
    [SerializeField] private CarriableBase outputPrefab;

    protected override bool CanProcess()
    {
        if (inputStack == null || outputStack == null) return false;
        if (outputPrefab == null) return false;

        if (!inputStack.HasItem(inputType)) return false;
        if (outputStack.IsFull) return false;

        return true;
    }

    protected override void ConsumeInput()
    {
        if (inputStack.TryTakeLastOfType(inputType, out CarriableBase item))
        {
            Destroy(item.gameObject);
        }
    }

    protected override void ProduceOutput()
    {
        CarriableBase spawned = Instantiate(outputPrefab, outputStack.transform);

        if (!outputStack.TryAdd(spawned))
        {
            Destroy(spawned.gameObject);
        }
    }
}