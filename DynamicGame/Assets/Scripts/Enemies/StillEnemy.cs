using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace Enemies
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class StillEnemy : EnemyBehaviourBase
    {
        public enum EnemyState
        {
            Quard,
            PlayerSeen,
            AlertOthers,
            ShootPlayer
        }

        public EnemyState CurrentState;

        [Header("Patroling targets")]
        [SerializeField] private Transform[] targets;
        private int destinationTarget = 0;

        protected override void Start()
        {
            base.Start();
           // gameObject.transform.position = targets[0].position;
        }

        protected override void Update()
        {
            base.Update();
            agent.speed = currentSpeed;

            switch (CurrentState)
            {
                case EnemyState.Quard:

                    Raycast();
                    currentSpeed = data.walkingSpeed;
                    agent.isStopped = false;
                    if (playerFound)
                        CurrentState = EnemyState.PlayerSeen;

                    if (!agent.pathPending && agent.remainingDistance < 0.5f)
                        Quarding();

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

                    CurrentState = EnemyState.Quard;

                    break;
            }
        }

        //Patrol between set points when player is not detected
        private void Quarding()
        {

        }

        // If player near by check surroundings every x seconds, display alert sign
        private void CheckSurroundings()
        {

        }
    }
}
