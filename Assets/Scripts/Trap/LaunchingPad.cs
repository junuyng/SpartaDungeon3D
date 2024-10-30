using System.Collections;
using UnityEngine;

public class LaunchingPad : Trap
{
    [SerializeField]private Transform launchDestination;
    [SerializeField]private  float decelerationForce = 0.5f;
     private const float arrivalThreshold = 1f;  

    
     protected override void ActivateTrap()
     {
         Transform playerPos = CharacterManager.Instance.player.transform;
         playerController.isOnTrap = true;

         playerPos.forward = (launchDestination.position - playerPos.position).normalized;

         Vector3 force = launchDestination.position - playerPos.position;
         playerController.AddForceToPlayer(force, ForceMode.Impulse);

         StartCoroutine(CheckPlayerArrive());
     }


    private IEnumerator CheckPlayerArrive()
    {
        while (Vector3.Distance(playerController.gameObject.transform.position, launchDestination.position) >arrivalThreshold)
        {
            yield return null;
        }
        
        //playerController.InitRigidbody();
        playerController.isOnTrap = false;
        
    }

  

}