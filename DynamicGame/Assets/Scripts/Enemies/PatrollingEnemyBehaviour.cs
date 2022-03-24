using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class PatrollingEnemyBehaviour : EnemyBehaviourBase
    {
        [Header("Patroling targets")]
        [SerializeField] protected Transform[] targets;
        private int destinationTarget = 0;

        protected  void LateUpdate()
        {
            base.Update();

            if (playerFound)
                currentState.CurrentState = EnemyStates.NPCSStateMachine.Chase;

            switch (currentState.CurrentState)
            {
                case EnemyStates.NPCSStateMachine.Idle:
                    {
                        if (isHit)
                        {
                            HitReaction();
                        }

                        break;
                    }

                case EnemyStates.NPCSStateMachine.Patrol:
                    {
                        if (distance < 2f && !player.GetComponent<Player.PlayerController>().isDisguised)
                        {
                            playerFound = true;
                            currentState.CurrentState = EnemyStates.NPCSStateMachine.Chase;
                        }

                        else if (isHit)
                        {
                            HitReaction();
                        }

                        else if (agent.isOnNavMesh && !agent.pathPending && agent.remainingDistance < 0.3f)
                            Patrolling();
                        break;
                    }

                case EnemyStates.NPCSStateMachine.Chase:
                    {
                        ChasePlayer();
                        break;
                    }

                case EnemyStates.NPCSStateMachine.Search:
                    {

                        LostTimer();
                        break;
                    }

                case EnemyStates.NPCSStateMachine.Return:
                    {
                        if (playerFound)
                            currentState.CurrentState = EnemyStates.NPCSStateMachine.Chase;

                        if(isHit)
                            HitReaction();

                        Return();
                        break;
                    }

                default:
                    currentState.CurrentState = EnemyStates.NPCSStateMachine.Patrol;
                    break;
            }
        }

        public override void Reset()
        {
            base.Reset();
            currentState.CurrentState = EnemyStates.NPCSStateMachine.Patrol;
            reset = false;

        }

        private void ChasePlayer()
        {
            currentSpeed = runSpeed;
            FollowPlayer();

            if (playerFound && !hitFront && !hitRight && !hitLeft && !hitUp)
                LostTimer();

            if (!playerFound)
                currentState.CurrentState = EnemyStates.NPCSStateMachine.Return;
        }

        private void Return()
        {
            currentSpeed = walkSpeed;
            agent.isStopped = false;
            agent.destination = targets[0].position;
            currentState.CurrentState = EnemyStates.NPCSStateMachine.Patrol;
        }

        //Patrol between set points when player is not detected
        protected virtual void Patrolling()
        {
            currentSpeed = walkSpeed;
            agent.isStopped = false;
            agent.destination = targets[destinationTarget].position;
            destinationTarget = (destinationTarget + 1) % targets.Length;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            for (int i = 0; i < targets.Length; i++)
            {
                Gizmos.DrawWireSphere(targets[i].position, 0.50f);

                if (i + 1 < targets.Length)
                    Gizmos.DrawLine(targets[i].position, targets[i + 1].position);
                else
                    Gizmos.DrawLine(targets[i].position, targets[0].position);
            }
        }
#endif

    }
}
