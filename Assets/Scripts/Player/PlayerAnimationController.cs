using System;
using UnityEngine;
 

public class PlayerAnimationController : AnimationController
{
    private int IsWalk = Animator.StringToHash("IsWalk");
    private PlayerController controller;

    protected override void Awake()
    {
        base.Awake();
        controller = GetComponent<PlayerController>();  
    }

    private void Start()
    {
        controller.OnMoveEvent += Move;
    }

    private void Move(Vector2 dir)
    {
        animator.SetBool(IsWalk,dir.magnitude> 0.5f);
    }

    private void OnDestroy()
    {
        controller.OnMoveEvent -= Move;
    }
}