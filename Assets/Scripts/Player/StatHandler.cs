using UnityEngine;

public class StatHandler : MonoBehaviour
{
    [SerializeField]private PlayerDataSO initStat;

    public int maxHp { get; private set; }
    public float moveSpeed{ get; private set; }
    public float jumpPower{ get; private set; }


    private void Awake()
    {
        maxHp = initStat.maxHp;
        moveSpeed = initStat.moveSpeed;
        jumpPower = initStat.jumpPower;
    }
    
}