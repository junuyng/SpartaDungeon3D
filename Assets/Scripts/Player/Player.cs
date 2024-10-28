using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
    }
}