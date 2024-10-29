using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    [Header("PlayerInteraction Components")] 
    [SerializeField] private Transform rayPos;
    
    [Header("PlayerInteraction Settings")] [SerializeField]
    private float interactDistance;
    [SerializeField] private float detectIndicatorObjectDelay;


    public event Action<string> OnInteractorEvent;
    private IInteractable currentInteractable;
    private Camera mainCamera;
    private PlayerCondition condition;
    
    private void Awake()
    {
        condition = GetComponent<PlayerCondition>();
    }
    
    private void Start()
    {
        mainCamera = Camera.main;
        StartCoroutine(MonitorInteractable());
    }
    
    // invoke는 성능이 안좋아서 InvokeRepeating보다는 코루틴 사용하는게 더 좋다 .
    private IEnumerator MonitorInteractable()
    {
        while (condition.CurHp > 0)
        {
            DetectInteractable();
            yield return new WaitForSeconds(detectIndicatorObjectDelay);
        }
    }

    private void DetectInteractable()
    {
        Vector3 rayOrigin = rayPos.position;  
        Vector3 rayDirection = mainCamera.transform.forward;  

        Ray ray = new Ray(rayOrigin, rayDirection);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit, interactDistance) || hit.collider == null )
        {
            ClearCurrentInteractable();
            return;
        }

        IInteractable interactable = hit.collider.GetComponent<IInteractable>();
        
        if (interactable == null)
        {
            ClearCurrentInteractable();
            return;
        }

        if(currentInteractable == interactable)
            return;
        
        currentInteractable = interactable;
        OnInteractorEvent?.Invoke(currentInteractable.GetInfo());
    }


    private void ClearCurrentInteractable()
    {
        currentInteractable = null;
        OnInteractorEvent?.Invoke("");  
    }

    private void OnDrawGizmos()
    {
        Vector3 rayOrigin = rayPos.position; // 플레이어 위치에서 약간 위로 Ray 시작
        Vector3 rayDirection = Camera.main.transform.forward;  

        Ray ray = new Ray(rayOrigin, rayDirection);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(ray.origin, ray.direction * interactDistance);
    }
}

