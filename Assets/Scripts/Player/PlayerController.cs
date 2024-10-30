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
    [SerializeField] private LayerMask wallLayerMask;
    [SerializeField] private float jumpPower;
    [SerializeField] private float wallDetectDistace; 
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
    public event Action<Vector2> OnSearchEvent;
    
 

    public bool isOnTrap = false;
    private bool canLook = true;
     
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inventory = GetComponent<PlayerInventory>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
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
        if (context.phase == InputActionPhase.Started )
        {
            if(IsGrounded() || IsWalled())
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

    public void OnSearch(InputAction.CallbackContext context)
    {
        context.ReadValue<Vector2>();
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

    public bool IsWalled()
    {
        Ray ray = new Ray(transform.position + (transform.up * 1f), transform.forward);
       
        if (Physics.Raycast(ray, wallDetectDistace, wallLayerMask))
        {
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

        Ray[] groundRays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < groundRays.Length; i++)
        {
            Gizmos.DrawRay(groundRays[i].origin, groundRays[i].direction * 0.1f);
        }
        
        Gizmos.color = Color.blue;
        Ray wallRay = new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 1f), transform.forward);
        Gizmos.DrawRay(wallRay.origin, wallRay.direction * wallDetectDistace);
    }

    public void AddForceToPlayer(Vector3 vec, ForceMode forceMode)
    {
        rb.velocity = Vector3.zero;

        Vector3 adjustedForce = (transform.forward * vec.z) + (transform.right * vec.x) + (Vector3.up * vec.y);
        rb.AddForce(adjustedForce, forceMode);
    }
    
    public void MovePositionToPlayer(Vector3 vec)
    {
        rb.MovePosition(transform.position+vec);
    }
    
    private void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }
}