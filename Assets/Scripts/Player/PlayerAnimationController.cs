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
        bool isMovingForward = dir.y >= 0.0f; // 옆으로 가는 것도 전진으로 처리 

        if (isMovingForward)
        {
            animator.SetBool(IsBackWalk, false);
            animator.SetBool(IsWalk, magnitude > 0.5f);
        }
        else
        {
            animator.SetBool(IsWalk, false);
            animator.SetBool(IsBackWalk, magnitude > 0.5f);
        }
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