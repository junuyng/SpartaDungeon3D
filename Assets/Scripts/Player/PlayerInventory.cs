using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{   
    
    [SerializeField]private List<Item> items = new List<Item>();
    
    public Item CurrentItem { get; private set; }
    public EquipmentItem EquipmentItem { get; private set; }

    private int itemIndex =0;
    private PlayerController controller;

    
    public event Action OnChangeItemEvent;
    public event Action OnAddItemEvent;
    public event Action OnUseItemEvent;
    public event Action OnEquipItemEvent;
    public event Action OnUnEquipItemEvent;

    
    private void Awake()
    {
        controller = GetComponent<PlayerController>();
    }

    private void Start()
    {
         controller.OnScrollEvent += ChangeItem;
    }


    public void RemoveEquipItem()
    {
        EquipmentItem.RemoveStat();
        AddItem(EquipmentItem);
        EquipmentItem = null;
        OnUnEquipItemEvent?.Invoke();
    }

    public void EquipItem(EquipmentItem equipmentItem)
    {
        if(EquipmentItem == null)
            EquipmentItem = equipmentItem;
        
        else
        {
            RemoveEquipItem();
            EquipmentItem = equipmentItem;
        }
        
        OnEquipItemEvent?.Invoke();
    }

    public void AddItem(Item item)
    {
        
        items.Add(item);

        if (CurrentItem == null)
            CurrentItem = items[0]; 

        OnAddItemEvent?.Invoke();
    }
    
    
    
    public void UseItem()
    {
        if(CurrentItem == null)
            return;
        
        CurrentItem.UseItem();
        items.Remove(CurrentItem);
        
        if (items.Count == 0)
        {
            CurrentItem = null;
        }
        
        else
        {
            itemIndex = itemIndex >= items.Count ? 0 : itemIndex;
            CurrentItem = items[itemIndex];
        }
        
        
        OnUseItemEvent?.Invoke();
    }

    
    public void ChangeItem(int value)
    {
       
        if (!IsRemainItem()) return;


        if (itemIndex + value >= items.Count)
            itemIndex = 0;
        
        else if (itemIndex + value < 0)
            itemIndex = items.Count - 1;
        
        else
        {
            itemIndex = itemIndex + value;
        }
        
        CurrentItem = items[itemIndex];
        OnChangeItemEvent?.Invoke();
    }

    
    public bool IsRemainItem()
    {
        return items.Count != 0;
    }


}