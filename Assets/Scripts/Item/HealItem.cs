using UnityEngine;

public class HealItem : ConsumableItem
{
    protected override void ExecuteConsumableItemUsage()
    {
        CharacterManager.Instance.player.condition.ChangeHp(ChangeHpType.Heal);
     }
}