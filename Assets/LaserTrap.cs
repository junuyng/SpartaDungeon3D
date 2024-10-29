using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LaserTrap : Trap
{
    [Header("LaserTrap Components")] [SerializeField]
    private Transform[] lasers;
    [SerializeField]private LayerMask targetLayer;

    private bool isLaserOn = false;


    private void Update()
    {
        if (isLaserOn)
        {
            DetectPlayerByRay();
        }
    }

    private void DetectPlayerByRay()
    {
        Ray[] rays = new Ray[lasers.Length];
        RaycastHit hit;
        for (int i = 0; i < rays.Length; i++)
        {
            Vector3 origin = lasers[i].GetChild(0).position;
            Vector3 direction = lasers[i].GetChild(1).position - origin;
            float maxDistance = direction.magnitude;
            direction.Normalize();

            if (Physics.Raycast(origin, direction, out hit, maxDistance, targetLayer))
            {
                hit.collider.GetComponent<PlayerCondition>().ChangeHp(ChangeHpType.Damage);
                isLaserOn = false;
            }

            Debug.DrawRay(origin, direction * maxDistance, Color.yellow);
        }
    }

    private void OnDrawGizmos()
    {
        if (lasers == null) return;

        for (int i = 0; i < lasers.Length; i++)
        {
            if (lasers[i] == null || lasers[i].childCount < 2) continue;

            Vector3 origin = lasers[i].GetChild(0).position;
            Vector3 direction = lasers[i].GetChild(1).position - origin;
            float maxDistance = direction.magnitude;
            direction.Normalize();

            Debug.DrawRay(origin, direction * maxDistance, Color.yellow);
        }
    }


    protected override void ActivateTrap()
    {
        isLaserOn = true;
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            isLaserOn = false;
        }
    }

}