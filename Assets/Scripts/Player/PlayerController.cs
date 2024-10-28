using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Ray = UnityEngine.Ray;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")] [SerializeField]
    private float moveSpeed;

    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private float jumpPower;
    private Rigidbody rigidbody;
    private Vector2 curMovementDir;

    [Header("Look Settings")] [SerializeField]
    private float minRotX;

    [SerializeField] private float maxRotX;
    [SerializeField] private float lookSensitivity;
    private Vector2 curMouseDelta;

    
    private const float LandingCheckDelay = 0.1f;

    public event Action<Vector2> OnMoveEvent;
    public event Action OnJumpEvent;
    public event Action OnLandEvent;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Move();
    }


    private void LateUpdate()
    {
    }

    private void Move()
    {
        Vector3 moveDir = transform.forward * curMovementDir.y + transform.right * curMovementDir.x;
        moveDir *= moveSpeed;
        moveDir.y = rigidbody.velocity.y;

        rigidbody.velocity = moveDir;
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
    
    private void PerformJump()
    {
        rigidbody.AddForce(Vector2.up * jumpPower,ForceMode.Impulse);
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
}