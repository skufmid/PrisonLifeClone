using UnityEngine;

public enum MineToolEquipType
{
    OneHand,
    TwoHand,
    Ride
}

[CreateAssetMenu(menuName = "Mining/Tool")]
public class MineTool : ScriptableObject
{
    public string toolName;

    public GameObject toolPrefab;
    public MineToolEquipType equipType;
    public Vector3 positionOffset;

    public float interval = 1f;
    public int maxMineAmount = 1;
    public float range = 1.5f;
}