using UnityEngine;

public class UI_FloorIndicator : MonoBehaviour
{
    [SerializeField] private float distanceFromPlayer = 2.5f;
    [SerializeField] private float yOffset = 0.05f;
    [SerializeField] private Transform player;
    private Renderer renderer;

    private Transform target;
    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
        renderer = GetComponentInChildren<Renderer>();

        Hide();
    }

    private void LateUpdate()
    {
        if (target == null || player == null || cam == null) return;

        UpdatePositionAndRotation();
    }

    public void Initialize(Transform playerTransform)
    {
        player = playerTransform;
    }

    public void Show(Transform targetTransform)
    {
        target = targetTransform;
        renderer.enabled = true;
    
    }

    public void Hide()
    {
        target = null;
        renderer.enabled = false;
    }

    private void UpdatePositionAndRotation()
    {
        Vector3 dir = target.position - player.position;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.001f) return;

        dir.Normalize();

        Vector3 pos = player.position + dir * distanceFromPlayer;
        pos.y += yOffset;

        transform.position = pos;

        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(90f, angle, 0f);
    }
}