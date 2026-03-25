using UnityEngine;

public class Indicator : MonoBehaviour
{
    public Transform target;

    [SerializeField] private Camera cam;

    [Header("Indicators")]
    [SerializeField] private UI_FloorIndicator floorIndicator;
    [SerializeField] private UI_AirIndicator airIndicator;

    private void Update()
    {
        if (cam == null) return;
        if (target == null)
        {
            floorIndicator.Hide();
            airIndicator.Hide();
            return;
        }

        bool isVisible = IsVisibleToCamera(target.position);

        if (isVisible)
        {
            airIndicator.Show(target);
            floorIndicator.Hide();
        }
        else
        {
            floorIndicator.Show(target);
            airIndicator.Hide();
        }
    }

    private bool IsVisibleToCamera(Vector3 worldPos)
    {
        Vector3 viewport = cam.WorldToViewportPoint(worldPos);

        return viewport.z > 0 &&
               viewport.x > 0 && viewport.x < 1 &&
               viewport.y > 0 && viewport.y < 1;
    }

    public void SetTartget(Transform newTarget)
    {
        target = newTarget;
    }

    public void SetTargetNull()
    {
        target = null;
    }
}