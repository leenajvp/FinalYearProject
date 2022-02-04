﻿using UnityEngine;
using UnityEngine.AI;
using Player;

[RequireComponent(typeof(NavMeshAgent))]

public class EnemyStates : MonoBehaviour
{
    public enum EnemyState
    {
        Patrol,
        PlayerSeen,
        ShootPlayer,
        AlertOthers
    }

    [Header("Patroling targets")]
    [SerializeField] private Transform[] targets;

    [Header("Enemy Speed Settings")]
    [SerializeField] private float walkingSpeed;
    [SerializeField] private float runningSpeed;
    private float currentSpeed;

    [Header("PlayerDetection Settings")]
    [SerializeField] private GameObject player;
    [SerializeField] private float shootDistance = 10;
    [SerializeField] private float lostDistance = 20f;

    private int destinationTarget = 0;
    private RaycastHit hit;
    public EnemyState CurrentState;
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
    }

    void Update()
    {
        agent.speed = currentSpeed;

        if (Physics.Raycast(transform.position, transform.forward, out hit))
        Debug.DrawRay(transform.position, transform.forward * 50);

        switch (CurrentState)
        {
            case EnemyState.Patrol:

                RaycastCheck();
                currentSpeed = walkingSpeed;
                agent.isStopped = false;

                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                    Patrolling();

                break;

            case EnemyState.PlayerSeen:
                
                ShootPlayer();
                currentSpeed = runningSpeed;

                break;

            case EnemyState.ShootPlayer:

                ShootPlayer();
                currentSpeed = runningSpeed;

                break;

            case EnemyState.AlertOthers:
                break;

            default:

                CurrentState = EnemyState.Patrol;

                break;
        }
    }

    void RaycastCheck()
    {
        if (hit.collider == null)
        {
            return;
        }

        else
        {

            if (hit.collider.gameObject == player)
            {
                CurrentState = EnemyState.PlayerSeen;
            }
        }
    }

    //patrol when player is lost between set positions
    void Patrolling()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.blue;
        agent.destination = targets[destinationTarget].position;
        destinationTarget = (destinationTarget + 1) % targets.Length;
    }


    // if other npcs detect player go towards
    void AlertMoreEnemies()
    {
        //gameObject.GetComponent<Renderer>().material.color = Color.yellow;

        //if (gameBoss.GetComponent<GameManager>().playerDetected == true)
        //{
        //    agent.SetDestination(player.transform.position);
        //}

        //else
        //{
        //    CurrentState = EnemyState.Patrol;
        //}
    }


    //Move towards player and stop when on shooting distance
    void ShootPlayer()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.red;
        agent.SetDestination(player.transform.position);

        if (Vector3.Distance(transform.position, player.transform.position) <= shootDistance)
        {
            CurrentState = EnemyState.ShootPlayer;
        }

        if (Vector3.Distance(transform.position, player.transform.position) >= lostDistance)
        {
            CurrentState = EnemyState.Patrol;
        }
    }
}
