 
using System;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    protected Animator animator;
    
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }
}