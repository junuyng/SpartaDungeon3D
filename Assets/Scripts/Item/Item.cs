using System;
using UnityEngine;



public abstract class Item : MonoBehaviour
{
    public ItemDataSO data;
    
    
    public void UseItem()
    {
        ExecuteItemUsage();
    }
    
    protected abstract void ExecuteItemUsage();
}