using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class ItemConverter : MonoBehaviour
{
    [SerializeField] private TileStack inputStack;
    [SerializeField] private TileStack outputStack;

    [SerializeField] private CarryItemType inputType;
    [SerializeField] private CarriableBase outputPrefab;

    [SerializeField] private float processTime = 1f;
    [SerializeField] private bool autoStart = true;

    [SerializeField] private UnityEvent onConverted;

    private Coroutine coProcess;
    private bool isProcessing;

    private void Update()
    {
        if (!autoStart) return;

        if (!isProcessing && CanProcess())
            coProcess = StartCoroutine(CoProcess());
    }

    private bool CanProcess()
    {
        if (inputStack == null || outputStack == null) return false;
        if (outputPrefab == null) return false;
        if (!inputStack.HasItem(inputType)) return false;
        if (outputStack.IsFull) return false;

        return true;
    }

    private IEnumerator CoProcess()
    {
        isProcessing = true;

        yield return new WaitForSeconds(processTime);

        if (inputStack.TryTakeLastOfType(inputType, out CarriableBase inputItem))
        {
            Destroy(inputItem.gameObject);

            CarriableBase outputItem = Instantiate(outputPrefab, outputStack.transform);
            if (!outputStack.TryAdd(outputItem))
            {
                Destroy(outputItem.gameObject);
            }
            else
            {
                onConverted?.Invoke();
            }
        }

        isProcessing = false;
        coProcess = null;
    }
}