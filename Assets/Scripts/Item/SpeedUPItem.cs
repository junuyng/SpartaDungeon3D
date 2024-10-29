using UnityEngine;

public class SpeedUPItem : ConsumableItem
{

    [Header(" SpeedUP Item Settings")] [SerializeField]
    private int value;

    [SerializeField] private float duration; 
    protected override void ExecuteConsumableItemUsage()
    {
         Debug.Log("SpeedUPItem 사용");
    }
}