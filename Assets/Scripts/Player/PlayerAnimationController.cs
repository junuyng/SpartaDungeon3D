using System;
using UnityEngine;


public class PlayerAnimationController : AnimationController
{
    
    private int IsWalk = Animator.StringToHash("IsWalk");
    private int IsBackWalk = Animator.StringToHash("IsBackWalk");

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
    
    
    private void OnDestroy()
    {
        controller.OnMoveEvent -= Move;
    }
}



