using UnityEngine;

public abstract class ConsumableItem : Item
{
    protected override void ExecuteItemUsage()
    {
        ExecuteConsumableItemUsage();
    }

    protected abstract  void ExecuteConsumableItemUsage();

}