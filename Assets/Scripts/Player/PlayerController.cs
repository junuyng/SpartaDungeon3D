using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Ray = UnityEngine.Ray;

public class PlayerController : MonoBehaviour
{
    private StatHandler statHandler;
    
    [Header("Movement Settings")] [SerializeField]
    private float moveSpeed;

    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private float jumpPower;
    private Rigidbody rb;
    private Vector2 curMovementDir;

    [Header("Look Settings")] 
    [SerializeField] private float minRotX;
    [SerializeField] private float maxRotX;
    [SerializeField] private float lookSensitivity;
    [SerializeField] private Transform cameraContainer;
    private Vector2 curMouseDelta;
    private float curRotX;
    private float curRotY;

    
    private const float LandingCheckDelay = 0.1f;
    public event Action<Vector2> OnMoveEvent;
    public event Action OnJumpEvent;
    public event Action OnLandEvent;
    
    //trap에 걸렸는지 확인하기 위한 필드
    public bool isOnTrap = false;
    
    private void Awake()
    {
        statHandler = GetComponent<StatHandler>();
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if(!isOnTrap)
            Move();
    }


    private void LateUpdate()
    {
        Look();
    }

    private void Look()
    {
        curRotX -= curMouseDelta.y * lookSensitivity;
        curRotY += curMouseDelta.x * lookSensitivity;

        curRotX = Mathf.Clamp(curRotX, minRotX, maxRotX);

        // 카메라의 X축 로컬 회전 (상하 회전)
        cameraContainer.localEulerAngles = new Vector3(curRotX, 0, 0);
        // 캐릭터의 Y축 회전 (좌우 회전)
        transform.eulerAngles = new Vector3(0, curRotY, 0);
    }

    
    private void Move()
    {
        Vector3 moveDir = transform.forward * curMovementDir.y + transform.right * curMovementDir.x;
        moveDir *= statHandler.moveSpeed;
        moveDir.y = rb.velocity.y;

        rb.velocity = moveDir;
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementDir = context.ReadValue<Vector2>();
        }

        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementDir = Vector2.zero;
        }

        OnMoveEvent?.Invoke(curMovementDir);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        curMouseDelta = context.ReadValue<Vector2>();
    }


    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            OnJumpEvent?.Invoke();
        }
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
        while (!IsGrounded())
        {
            yield return null;
        }
        OnLandEvent?.Invoke();
    }


    private bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }
        
        return false;
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down ),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            Gizmos.DrawRay(rays[i].origin, rays[i].direction * 0.1f);
        }
    }


    public void AddForceToPlayer(Vector3 vec,ForceMode forceMode)
    {
        rb.velocity = Vector3.zero;
      
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        Vector3 adjustedForce = (cameraForward * vec.z) + (cameraRight * vec.x) + (Vector3.up * vec.y);
        rb.AddForce(adjustedForce,forceMode);
    }
}