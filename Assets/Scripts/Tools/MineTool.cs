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

    public MineToolEquipType equipType;
    public GameObject toolPrefab;

    public float interval = 1f;
    public int maxMineAmount = 1;
    public float range = 1.5f;
}