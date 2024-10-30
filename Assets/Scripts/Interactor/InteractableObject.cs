using UnityEngine;


public  class InteractableObject : MonoBehaviour , IInteractable
{
    public string GetInfo()
    {
        return $"상점입니다";
    }

    public void OnInteract()
    {
    }

    
    public void InteractBySearch()
    {
    }
    
    public void UnInteractBySearch()
    {
    }
}