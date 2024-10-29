using System.Collections;
using UnityEngine;

public enum StatType
{
    MaxHp,
    MoveSpeed,
    JumpPower,
    ClimbSpeed
}

public enum AdjustType
{
    Permanent,
    Temporary
}

public class StatHandler : MonoBehaviour
{
    [SerializeField]private PlayerDataSO initStat;

    public int maxHp { get; private set; }
    public float moveSpeed{ get; private set; }
    public float jumpPower{ get; private set; }
    public float climbSpeed{ get; private set; }

    private void Awake()
    {
        maxHp = initStat.maxHp;
        moveSpeed = initStat.moveSpeed;
        jumpPower = initStat.jumpPower;
        climbSpeed = initStat.climbSpeed;
    }

    
    public void UpdateStat(StatType statType, AdjustType adjustType, float value, float duration = 0f)
    {
        if (adjustType == AdjustType.Permanent)
        {
            ApplyStatChange(statType, value);
        }
        else if (adjustType == AdjustType.Temporary)
        {
            ApplyStatChange(statType, value);
            StartCoroutine(RevertStatAfterDuration(statType, value, duration));
        }
    }

    private void ApplyStatChange(StatType statType, float value)
    {
        switch (statType)
        {
            case StatType.MaxHp:
                maxHp += (int)value;
                break;
            case StatType.MoveSpeed:
                moveSpeed += value;
                break;
            case StatType.JumpPower:
                jumpPower += value;
                break;
            case StatType.ClimbSpeed:
                climbSpeed += value;
                break;
        }
    }

    private IEnumerator RevertStatAfterDuration(StatType statType, float value, float duration)
    {
        yield return new WaitForSeconds(duration);
        ApplyStatChange(statType, -value);  
    }


}