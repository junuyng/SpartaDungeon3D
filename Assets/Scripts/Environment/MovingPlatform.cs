using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovingPlatform : MonoBehaviour
{
    [Header("MovingPlatform Settings")] [SerializeField]
    private float nextMoveDelay = 2f; // 이동 간 딜레이

    [SerializeField] private float moveSpeed = 2f; // 이동 속도
    [SerializeField] private Transform targetPos1;
    [SerializeField] private Transform targetPos2;

    private Rigidbody rb;

    private Vector3 direction;


    private Transform destination;
    private bool isMoving = true;
    private bool isPlayerMove = false;

    private Coroutine playerMoveCheckCoroutine;

    private const float checkPlayerMoveDelay = 0.1f;
    private Vector3 lastPosition;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;  
        InitPlatform();
        CharacterManager.Instance.player.controller.OnMoveEvent += OnPlayerMove;
    }


    private void OnPlayerMove(Vector2 dir)
    {
        if (playerMoveCheckCoroutine != null)
        {
            StopCoroutine(playerMoveCheckCoroutine);
        }

        playerMoveCheckCoroutine = StartCoroutine(CheckPlayerMove(dir));
    }

    IEnumerator CheckPlayerMove(Vector2 dir)
    {
        if (dir != Vector2.zero)
        {
            isPlayerMove = true;
            yield break;
        }

        yield return new WaitForSeconds(checkPlayerMoveDelay);
        isPlayerMove = false;
    }


    private void FixedUpdate()
    {
        if (isMoving)
        {
            if (Vector3.Distance(transform.position, destination.position) < 0.1f)
            {
                isMoving = false;
                StartCoroutine(ChangeDestinationAfterDelay());
            }
            else
            {
                rb.MovePosition(transform.position + direction * moveSpeed * Time.fixedDeltaTime);
            }
        }

        lastPosition = transform.position;
    }

    private void InitPlatform()
    {
        destination = targetPos2;
        direction = (destination.position - transform.position).normalized;
    }

    private IEnumerator ChangeDestinationAfterDelay()
    {
        yield return new WaitForSeconds(nextMoveDelay);

        destination = destination == targetPos2 ? targetPos1 : targetPos2;
        direction = (destination.position - transform.position).normalized;

        isMoving = true;
    }


    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && !isPlayerMove && isMoving)
        {
            Vector3 platformMovement = transform.position - lastPosition; 
            Rigidbody playerRb = other.gameObject.GetComponent<Rigidbody>();
            playerRb.MovePosition(playerRb.position + direction * moveSpeed * Time.fixedDeltaTime);
        }
    }
}