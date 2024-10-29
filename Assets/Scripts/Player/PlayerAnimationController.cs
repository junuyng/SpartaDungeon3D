using System;
using System.Collections;
using UnityEngine;


public class PlayerAnimationController : AnimationController
{
    private int IsWalk = Animator.StringToHash("IsWalk");
    private int IsBackWalk = Animator.StringToHash("IsBackWalk");
    private int Jumping = Animator.StringToHash("Jumping");
    private int Landing = Animator.StringToHash("Landing");

    private PlayerController controller;
    private bool isJumping = false;

    protected override void Awake()
    {
        base.Awake();
        controller = GetComponent<PlayerController>();
    }

    private void Start()
    {
        controller.OnMoveEvent += Move;
        controller.OnJumpEvent += Jump;
        controller.OnLandEvent += Land;
    }


    private void Move(Vector2 dir)
    {
        float magnitude = dir.magnitude;
        bool isMove= magnitude > 0.0f;  
        
        animator.SetBool(IsWalk, isMove);
        animator.SetFloat("WorkMagnitude", dir.y);
    }
    
    private void Jump()
    {
        if(isJumping)
            return;
        
        isJumping = true;
        animator.SetTrigger(Jumping);
    }

    private void Land()
    {
        isJumping = false;
        animator.SetTrigger(Landing);
    }
    
    private void OnDestroy()
    {
        controller.OnMoveEvent -= Move;
    }
}