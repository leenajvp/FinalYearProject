﻿using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class PatrollingEnemyBehaviour : EnemyBehaviourBase
    {
        public enum EnemyState
        {
            Patrol,
            PlayerSeen,
            ShootPlayer,
            AlertOthers
        }

        public EnemyState CurrentState;

        [Header("Patroling targets")]
        [SerializeField] private Transform[] targets;
        private int destinationTarget = 0;

        protected override void Start()
        {
            base.Start();
            gameObject.transform.position = targets[0].position;
        }

        void Update()
        {
            agent.speed = currentSpeed;

            switch (CurrentState)
            {
                case EnemyState.Patrol:

                    Raycasting();
                    currentSpeed = data.walkingSpeed;
                    agent.isStopped = false;
                    if (playerFound)
                        CurrentState = EnemyState.PlayerSeen;

                    if (!agent.pathPending && agent.remainingDistance < 0.5f)
                        Patrolling();

                    break;

                case EnemyState.PlayerSeen:

                    FollowPlayer();
                    currentSpeed = data.runningSpeed;

                    break;

                case EnemyState.ShootPlayer:

                    FollowPlayer();
                    currentSpeed = data.runningSpeed;

                    break;

                case EnemyState.AlertOthers:
                    // get enemies in radius and get them target the player
                    break;

                default:

                    CurrentState = EnemyState.Patrol;

                    break;
            }
        }

        //Patrol between set points when player is not detected
        private void Patrolling()
        {



            agent.destination = targets[destinationTarget].position;
            destinationTarget = (destinationTarget + 1) % targets.Length;

        }

    }
}
