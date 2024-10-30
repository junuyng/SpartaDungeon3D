
using System;
using UnityEngine;
using UnityEngine.LowLevel;

public  class EquipmentItem : Item
{
    [SerializeField]private StatType statType;
    [SerializeField] private float statValue;
    

    public virtual void AddStat()
    {
        CharacterManager.Instance.player.statHandler.UpdateStat(statType,AdjustType.Permanent,statValue);
    }
    
    public virtual void RemoveStat()
    {
        CharacterManager.Instance.player.statHandler.UpdateStat(statType,AdjustType.Permanent,-statValue);
    }
    
    
    protected override void ExecuteItemUsage()
    {
        CharacterManager.Instance.player.inventory.EquipItem(this);
        AddStat();
    }

  }