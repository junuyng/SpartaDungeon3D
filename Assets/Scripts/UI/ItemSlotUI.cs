
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI: MonoBehaviour
{

    [SerializeField] protected Image icon ;
    
    private int itemIndex = 0;
     

    protected virtual void Start()
    {
        icon.enabled = false;
        CharacterManager.Instance.player.inventory.OnChangeItemEvent += UpdateItemIcon;
        CharacterManager.Instance.player.inventory.OnUseItemEvent += UpdateItemIcon;
    }


    protected void UpdateItemIcon()
    {
        
        if (CharacterManager.Instance.player.inventory.CurrentItem == null)
        {
            icon.enabled = false;
            icon.sprite = null;
            return;
        }
        
        if(!icon.enabled )
             icon.enabled = true;
       
        icon.sprite = CharacterManager.Instance.player.inventory.CurrentItem.data.icon;
    }

}
