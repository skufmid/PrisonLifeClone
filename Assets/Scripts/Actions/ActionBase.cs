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

    public virtual bool TryStart(TTarget newTarget)
    {
        if (IsActing) return false;
        if (newTarget == null) return false;
        if (!CanStart(newTarget)) return false;

        target = newTarget;

        OnStarted();
        actionCoroutine = StartCoroutine(CoAction());
        return true;
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