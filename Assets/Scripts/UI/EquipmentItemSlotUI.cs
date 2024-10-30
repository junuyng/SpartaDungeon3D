using UnityEngine;
using UnityEngine.UI;

public class EquipmentItemSlotUI : ItemSlotUI
{

    protected virtual void Start()
    {
        icon.enabled = false;
        CharacterManager.Instance.player.inventory.OnEquipItemEvent += UpdateItemIcon;
        CharacterManager.Instance.player.inventory.OnUnEquipItemEvent += UpdateItemIcon;
    }
    
    
}