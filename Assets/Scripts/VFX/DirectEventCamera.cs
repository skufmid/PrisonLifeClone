using Cinemachine;
using System.Collections;
using UnityEngine;

public class DirectEventCamera : MonoBehaviour
{
    private CinemachineVirtualCamera eventCamera;
    [SerializeField] private float focusDuration = 2f;

    private Coroutine routine;

    private void Awake()
    {
        eventCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void FocusEventTarget(Transform target)
    {
        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(CoFocusEventTarget(target));
    }

    private IEnumerator CoFocusEventTarget(Transform target)
    {
        GameManager.Instance.player.IsMovable = false;
        if (target != null)
        {
            eventCamera.Follow = target;
            eventCamera.LookAt = target;
        }
        eventCamera.Priority = 20;
        yield return new WaitForSeconds(focusDuration);

        eventCamera.Priority = 0;
        GameManager.Instance.player.IsMovable = true;
        routine = null;
    }
}