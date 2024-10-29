using UnityEngine;

public abstract class Trap : MonoBehaviour
{
    protected PlayerCondition playerCondition;
    protected PlayerController playerController;

    protected virtual void Start()
    {
        playerCondition = CharacterManager.Instance.player.condition;
        playerController = CharacterManager.Instance.player.controller;
    }

    protected abstract void ActivateTrap();
    
    protected void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
             ActivateTrap();
        }
    }
    
}