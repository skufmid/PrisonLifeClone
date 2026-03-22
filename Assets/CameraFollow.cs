using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;   // 따라갈 대상
    public Vector3 offset;     // 카메라 위치 오프셋

    private void Awake()
    {
        offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        if (target == null) return;

        transform.position = target.position + offset;
    }
}