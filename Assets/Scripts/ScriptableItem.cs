using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableItem", menuName = "Item/ScriptableItem", order = 1)]
public class ScriptableItem : ScriptableObject
{
    public string identifier;
    [Header("World Item")]
    public GameObject ItemPrefab;
    [Header("Inventory")]
    public Sprite InventorySprite;
    [Header("Visual Inventory")]
    public bool canUseBeltslots;
    public Vector3 RotationLeftBeltslots;
    public Vector3 PositionOffsetLeftBeltslots;
    public Vector3 RotationRightBeltslots;
    public Vector3 PositionOffsetRightBeltslots;
    [Header(" ")]
    public bool canUseLowerBackslot;
    public Vector3 RotationLowerBackslot;
    public Vector3 PositionOffsetLowerBackslot;
}
