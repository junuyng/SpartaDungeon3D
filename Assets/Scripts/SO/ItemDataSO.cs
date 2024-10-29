
using UnityEngine;

public enum ItemType 
{
    Consumable,
    Equip
}

[CreateAssetMenu(fileName = "ItemDataSO", menuName = "Item/ItemData")]
public class ItemDataSO : ScriptableObject
{
    [Header("Info")] 
    public string displayName;
    public string description;
    public ItemType type;
    public Sprite icon;
    
    [Header("Stacking")] 
    public bool canStack;
    public int maxStackAmount;

    [Header("Consumable")] 
    public ConsumableItem[] consumables;
    
    [Header("Equip")]
    public GameObject equipPrefab;
}