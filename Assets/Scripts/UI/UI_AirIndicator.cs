using UnityEngine;

public class UI_AirIndicator : MonoBehaviour
{
    [SerializeField] private Vector3 offset = new Vector3(0, 2f, 0);
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
        if (target == null) return;

        transform.position = target.position + offset;
    }

    public void Show(Transform t)
    {
        target = t;
        renderer.enabled = true;
    }

    public void Hide()
    {
        target = null;
        renderer.enabled = false;
    }
}