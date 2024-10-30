using UnityEngine;

public class SpeedUPItem : ConsumableItem
{

    [Header(" SpeedUP Item Settings")] [SerializeField]
    private int value;

    [SerializeField] private float duration; 
    protected override void ExecuteConsumableItemUsage()
    {
        CharacterManager.Instance.player.statHandler.UpdateStat(StatType.MoveSpeed,AdjustType.Temporary,value,duration);
    }
    
}