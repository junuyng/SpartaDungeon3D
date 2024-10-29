using UnityEngine;

public class JumpPad : Trap
{
    [Header("Jump Pad Settings")]
    [Tooltip("좌우로 가는 힘")]
    [SerializeField]private float jumpForceX; 
    [Tooltip("상하로 가는 힘")]
    [SerializeField]private float jumpForceY;
    [Tooltip("앞뒤가는 힘")]
    [SerializeField]private float jumpForceZ;
    
    
    protected override  void ActivateTrap()
    {
        playerController.isOnTrap = true;
         Vector3 jumpDir = new Vector3(jumpForceX, jumpForceY, jumpForceZ);
        playerController.AddForceToPlayer(jumpDir , ForceMode.Impulse);
        Invoke(nameof(UnActivateTrap),1f);
        
        //TODO JumpPad 사용시 Jump 애니메이션으로 변경
    }

    private void UnActivateTrap()
    {
        playerController.isOnTrap = false;
    }
}