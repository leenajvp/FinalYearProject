using System.Collections;
using UnityEngine;
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
        [SerializeField] protected Transform[] targets;
        private int destinationTarget = 0;

        protected override void Start()
        {
            base.Start();
            gameObject.transform.position = targets[0].position;
        }

        protected override void Update()
        {
            base.Update();
            agent.speed = currentSpeed;

            switch (CurrentState)
            {
                case EnemyState.Patrol:
                    agent.speed = data.walkingSpeed;
                    agent.isStopped = false;
                    Raycast();

                    if (!agent.pathPending && agent.remainingDistance < 0.5f)
                        Patrolling();

                    if (playerFound)
                    {
                        CurrentState = EnemyState.PlayerSeen;
                    }

                    break;

                case EnemyState.PlayerSeen:

                    FollowPlayer();

                    if (!playerFound)
                    {
                        agent.destination = targets[0].position;
                        CurrentState = EnemyState.Patrol;
                    }

                    break;

                case EnemyState.ShootPlayer:

                    FollowPlayer();

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
        protected virtual void Patrolling()
        {
            agent.destination = targets[destinationTarget].position;
            destinationTarget = (destinationTarget + 1) % targets.Length;

            if (isHit)
                StartCoroutine(ReactionTimer());
        }

        private IEnumerator ReactionTimer()
        {
            yield return new WaitForSeconds(1);
            CurrentState = EnemyState.ShootPlayer;
            isHit = false;
        }

        private void OnDrawGizmosSelected()
        {
            for (int i = 0; i < targets.Length; i++)
            {
                Gizmos.DrawWireSphere(targets[i].position, 0.30f);

                if (i + 1 < targets.Length)
                    Gizmos.DrawLine(targets[i].position, targets[i + 1].position);
                else
                    Gizmos.DrawLine(targets[i].position, targets[0].position);
            }
        }

    }
}
