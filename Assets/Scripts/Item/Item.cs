using System;
using UnityEngine;



public abstract class Item : MonoBehaviour
{
    public ItemDataSO data;
    private PlayerController playerController;
    
    protected void Awake()
    {
    }
    
    
    public void UseItem()
    {
        ExecuteItemUsage();
    }
    
    protected abstract void ExecuteItemUsage();
}