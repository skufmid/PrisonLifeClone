using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StackSlotData
{
    public CarrySlotType slotType;
    public Transform anchor;
    public int rows = 1;
    public int cols = 1;
    public int maxLayers = 10;
    public Vector3 itemSpacing = new Vector3(0.45f, 0.3f, 0.45f);
    public bool centerAlign = true;

    [Header("Rotation Option")]
    public bool useCustomRotation = false;
    public Vector3 rotationEuler = Vector3.zero;

    [HideInInspector] public List<CarriableBase> items = new();

    public int CapacityPerLayer => rows * cols;
    public int MaxCapacity => rows * cols * maxLayers;
}