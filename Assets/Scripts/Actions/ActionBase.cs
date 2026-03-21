using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class ActionBase<TTarget> : MonoBehaviour where TTarget : Component
{
    protected CharacterBase owner;
    protected Coroutine actionCoroutine;
    protected TTarget target;

    protected virtual void Awake()
    {
        owner = GetComponent<CharacterBase>();
    }

    public bool IsActing => actionCoroutine != null;

    protected virtual void OnTriggerEnter(Collider other)
    {
        TTarget target = other.GetComponent<TTarget>();
        TryStart(target);
    }

    public virtual void TryStart(TTarget newTarget)
    {
        if (IsActing) return;
        if (newTarget == null) return;
        if (!CanStart(newTarget)) return;

        target = newTarget;

        OnStarted();
        actionCoroutine = StartCoroutine(CoAction());

    }
    protected virtual bool CanStart(TTarget target)
    {
        return true;
    }

    public virtual void StopAction()
    {
        if (!IsActing) return;

        StopCoroutine(actionCoroutine);
        actionCoroutine = null;
        target = null;

        OnStopped();
    }

    protected virtual IEnumerator CoAction()
    {
        return null;
    }

    protected virtual void OnStarted() { }
    protected virtual void OnStopped() { }
}