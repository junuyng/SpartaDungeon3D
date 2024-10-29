using UnityEngine;


public  class ShopObject : MonoBehaviour , IInteractable
{
    public string GetInfo()
    {
        return $"상점입니다";
    }

    public void OnInteract()
    {
    }
}