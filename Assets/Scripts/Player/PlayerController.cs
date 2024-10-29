using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Ray = UnityEngine.Ray;

public class PlayerController : MonoBehaviour
{
    private PlayerInventory inventory;

    [Header("Movement Settings")] [SerializeField]
    private float moveSpeed;

    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private float jumpPower;
    private Rigidbody rb;

    [Header("Look Settings")] [SerializeField]
    private float minRotX;

    [SerializeField] private float maxRotX;
    [SerializeField] private float lookSensitivity;
    [SerializeField] private Transform cameraContainer;
    private Vector2 curMouseDelta;
    private float curRotX;
    private float curRotY;


    public float useItemDelay = 0.5f;
    private float lastItemUseTime;

    public event Action<Vector2> OnMoveEvent;
    public event Action OnJumpEvent;
    public event Action OnLandEvent;
    public event Action<int> OnScrollEvent;
    public event Action OnUseItemEvent;


    //trap에 걸렸는지 확인하기 위한 필드
    public bool isOnTrap = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inventory = GetComponent<PlayerInventory>();
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


    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            OnMoveEvent?.Invoke(context.ReadValue<Vector2>());
        }

        else if (context.phase == InputActionPhase.Canceled)
        {
            OnMoveEvent?.Invoke(Vector2.zero);
        }
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


    public void OnUseItem(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && CanUseItem() && inventory.IsRemainItem())
        {
            inventory.UseItem();
        }
    }


    public void OnScroll(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            float scrollY = context.ReadValue<Vector2>().y;

            if (scrollY == 0f) return;

            int value = scrollY > 0 ? 1 : -1;

            OnScrollEvent?.Invoke(value);
        }
    }


    public void CallOnLandingEvent()
    {
        OnLandEvent?.Invoke();
    }


    public bool CanUseItem()
    {
        if (Time.time - lastItemUseTime >= useItemDelay)
        {
            lastItemUseTime = Time.time;
            return true;
        }

        return false;
    }

    public bool IsGrounded()
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
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            Gizmos.DrawRay(rays[i].origin, rays[i].direction * 0.1f);
        }
    }

    public void AddForceToPlayer(Vector3 vec, ForceMode forceMode)
    {
        rb.velocity = Vector3.zero;

        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        Vector3 adjustedForce = (cameraForward * vec.z) + (cameraRight * vec.x) + (Vector3.up * vec.y);
        rb.AddForce(adjustedForce, forceMode);
    }
    
    public void MovePositionToPlayer(Vector3 vec)
    {
        
        rb.MovePosition(transform.position+vec);
    }
}