using UnityEngine;

public class UI_FloorIndicator : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform target;
    [SerializeField] private Camera targetCamera;
    [SerializeField] private GameObject arrowVisual;
    [SerializeField] private float distanceFromPlayer = 2f;
    [SerializeField] private float groundY = 0.05f;
    [SerializeField] private float hideDistance = 1f;

    private void Reset()
    {
        targetCamera = Camera.main;
        arrowVisual = gameObject;
    }

    private void Awake()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }

        if (arrowVisual == null)
        {
            arrowVisual = gameObject;
        }
    }

    private void LateUpdate()
    {
        if (player == null || target == null || targetCamera == null)
        {
            SetArrowActive(false);
            return;
        }

        Vector3 toTarget = target.position - player.position;
        toTarget.y = 0f;

        if (toTarget.sqrMagnitude <= hideDistance * hideDistance)
        {
            SetArrowActive(false);
            return;
        }

        bool isVisible = IsTargetVisible(target.position);

        if (isVisible)
        {
            SetArrowActive(false);
            return;
        }

        SetArrowActive(true);
        UpdateArrowTransform(toTarget);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void UpdateArrowTransform(Vector3 toTarget)
    {
        Vector3 direction = toTarget.normalized;
        Vector3 position = player.position + direction * distanceFromPlayer;
        position.y = groundY;

        transform.position = position;

        if (direction.sqrMagnitude > 0.0001f)
        {
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
    }

    private bool IsTargetVisible(Vector3 worldPosition)
    {
        Vector3 viewportPoint = targetCamera.WorldToViewportPoint(worldPosition);

        if (viewportPoint.z <= 0f) return false;
        if (viewportPoint.x < 0f || viewportPoint.x > 1f) return false;
        if (viewportPoint.y < 0f || viewportPoint.y > 1f) return false;

        return true;
    }

    private void SetArrowActive(bool active)
    {
        if (arrowVisual.activeSelf == active) return;
        arrowVisual.SetActive(active);
    }
}