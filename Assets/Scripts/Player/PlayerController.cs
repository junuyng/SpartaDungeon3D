using System;
using UnityEngine;
using UnityEngine.InputSystem;
 
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    
    [Header("Movement Settings")] 
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower;
    private Rigidbody rigidbody;
    private Vector2 curMovementDir;

    [Header("Look Settings")] 
    [SerializeField] private float minRotX;
    [SerializeField] private float maxRotX;
    [SerializeField] private float lookSensitivity;
    private Vector2 curMouseDelta;
    Camera camera = Camera.main;
    
    
    public event Action<Vector2> OnMoveEvent;   

    
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
        throw new NotImplementedException();
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

}