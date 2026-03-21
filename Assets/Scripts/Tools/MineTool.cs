using UnityEngine;

[CreateAssetMenu(menuName = "Mining/Tool")]
public class MineTool : ScriptableObject
{
    public string toolName;

    public float interval = 1f;          // 채굴 간격
    public float maxMineAmount = 1f;     // 1회 최대 채굴량
    public float range = 1.5f;           // 도구 크기 (반경)
}