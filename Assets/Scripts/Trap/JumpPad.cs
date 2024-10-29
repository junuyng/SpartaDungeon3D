using UnityEngine;

public class JumpPad : Trap
{
    [Tooltip("좌우로 가는 힘 (X축)")]
    [SerializeField] private float jumpForceX; 

    [Tooltip("위로 가는 힘 (Y축)")]
    [SerializeField] private float jumpForceY;

    [Tooltip("앞뒤로 가는 힘 (Z축)")]
    [SerializeField] private float jumpForceZ;

    [SerializeField] private float jumpPadDuration = 1f;
    
    protected override  void ActivateTrap()
    {
        playerController.isOnTrap = true;
         Vector3 jumpDir = new Vector3(jumpForceX, jumpForceY, jumpForceZ);
        playerController.AddForceToPlayer(jumpDir , ForceMode.Impulse);
        Invoke(nameof(UnActivateTrap),jumpPadDuration);
        
        //TODO JumpPad 사용시 Jump 애니메이션으로 변경
    }

    private void UnActivateTrap()
    {
        playerController.isOnTrap = false;
    }
}