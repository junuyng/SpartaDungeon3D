
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI: MonoBehaviour
{

    [SerializeField] private Image icon ;
    
    private int itemIndex = 0;
    private PlayerInventory playerInventory;

    private void Awake()
    {
        playerInventory = CharacterManager.Instance.player.inventory;
    }

    private void Start()
    {
        icon.enabled = false;
        CharacterManager.Instance.player.inventory.OnChangeItemEvent += UpdateItemIcon;
        CharacterManager.Instance.player.inventory.OnUseItemEvent += UpdateItemIcon;
    }


    private void UpdateItemIcon()
    {
        Debug.Log(playerInventory.CurrentItem);
        
        if (playerInventory.CurrentItem == null)
        {
            icon.enabled = false;
            icon.sprite = null;
            return;
        }
        
        if(!icon.enabled )
             icon.enabled = true;
       
        icon.sprite = playerInventory.CurrentItem.data.icon;
    }

}
