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
    [SerializeField] private LayerMask interactableBySearchLayerMask; // 인스펙터에서 플레이어 제외할 레이어 설정

    public event Action<string> OnInteractorEvent;
    public event Action<IInteractable> OnInteractorSearcEvent;

    private IInteractable currentInteractable;
    private Camera mainCamera;
    private PlayerCondition condition;
    private PlayerController controller;
    private Vector2 curMousePos;
    private IInteractable currentInteractableBySearch;

    private void Awake()
    {
        condition = GetComponent<PlayerCondition>();
        controller = GetComponent<PlayerController>();
    }
    
    private void Start()
    {
        mainCamera = Camera.main;
        controller.OnSearchEvent += SetMousePos;
        StartCoroutine(MonitorInteractable());
    }


    private void SetMousePos(Vector2 vec)
    {
        curMousePos = vec;
    }

    private void SearchInteractableObject()
    {
       Ray ray = mainCamera.ScreenPointToRay(curMousePos);
       RaycastHit hit;

       if (Physics.Raycast(ray, out hit, 100,interactableBySearchLayerMask))
       {
           
           GameObject targetObject = hit.collider.gameObject;
           IInteractable interactable =  targetObject.GetComponent<IInteractable>();

           if (interactable!= null && currentInteractableBySearch != interactable)
           {
               if(currentInteractableBySearch != null)
                    currentInteractableBySearch.UnInteractBySearch();
               
               currentInteractableBySearch = interactable;
               interactable.InteractBySearch();
               OnInteractorSearcEvent?.Invoke(interactable);
           }
           
           else if (interactable == null && currentInteractableBySearch != null)
           {
               currentInteractableBySearch.UnInteractBySearch();
               currentInteractableBySearch = null;
           }
       }
       
       Debug.DrawRay(ray.origin, ray.direction * 100, Color.magenta);
    }



    // invoke는 성능이 안좋아서 InvokeRepeating보다는 코루틴 사용하는게 더 좋다 .
    private IEnumerator MonitorInteractable()
    {
        while (condition.CurHp > 0)
        {
            DetectInteractable();
            SearchInteractableObject();
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

