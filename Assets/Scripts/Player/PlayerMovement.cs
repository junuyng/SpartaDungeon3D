using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("PlayerMovement Components")]
    private Rigidbody rb;
    private PlayerController controller;
    private StatHandler statHandler;
    
    private Vector2 curMovementDir;
    private const float LandingCheckDelay = 0.1f;

    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();  
        statHandler = GetComponent<StatHandler>();
        controller = GetComponent<PlayerController>();
    }

    private void Start()
    {
        controller.OnMoveEvent += SetMovementDirection;
    }

    private void FixedUpdate()
    {
        if(!controller.isOnTrap)
            Move();
    }


    private void SetMovementDirection(Vector2 dir)
    {
        curMovementDir = dir; 
    }
    

    
    private void Move()
    {
        Vector3 moveDir = transform.forward * curMovementDir.y + transform.right * curMovementDir.x;
        moveDir *= statHandler.moveSpeed;
        moveDir.y = rb.velocity.y;
        rb.velocity = moveDir;
    }
    
    
    
    //애니메이션 이벤트에 등록해서 사용    
    private void PerformJump()
    {
        rb.AddForce(Vector2.up * statHandler.jumpPower,ForceMode.Impulse);
        StartCoroutine(CheckLanding());
        
    }
    
    
    
    private IEnumerator CheckLanding()
    {
        yield return new WaitForSeconds(LandingCheckDelay);
        while (!controller.IsGrounded())
        {
            yield return null;
        }

        controller.CallOnLandingEvent();
    }
    
    
    
}
