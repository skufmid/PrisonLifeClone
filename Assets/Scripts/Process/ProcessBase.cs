using System.Collections;
using UnityEngine;

public abstract class ProcessorBase : MonoBehaviour
{
    [Header("Stack")]
    [SerializeField] protected TileStack inputStack;
    [SerializeField] protected TileStack outputStack;

    [Header("Process")]
    [SerializeField] protected float processTime = 1f;
    [SerializeField] protected bool autoStart = true;

    protected Coroutine coProcess;
    protected bool isProcessing;

    protected virtual void Update()
    {
        if (!autoStart) return;
        TryStartProcess();
    }

    public void TryStartProcess()
    {
        if (isProcessing) return;
        if (!CanProcess()) return;

        coProcess = StartCoroutine(CoProcess());
    }

    private IEnumerator CoProcess()
    {
        isProcessing = true;

        yield return new WaitForSeconds(processTime);

        ConsumeInput();
        ProduceOutput();

        isProcessing = false;
        coProcess = null;
    }

    protected abstract bool CanProcess();
    protected abstract void ConsumeInput();
    protected abstract void ProduceOutput();
}