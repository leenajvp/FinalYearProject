using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class GuardingEnemy : EnemyBehaviourBase
    {
        [Header("NPC look around")]
        [SerializeField] private float turnAngle = 45.0f;
        [SerializeField] private float rotationSpeed = 0.01f;
        [SerializeField] private float returnGuardDistance = 0.3f;
        private float newTurnAngle;
        private float defaultRot;
        private float currentRot;
        private float turnTime = 0;
        public bool turned;

        protected override void Start()
        {
            base.Start();
            defaultRot = transform.localEulerAngles.y;
            currentRot = transform.localEulerAngles.y;
            returnGuardDistance = data.returnGuardDistance;
            InvokeRepeating("TurnTimer", 0, 4);
        }

        protected  void LateUpdate()
        {
            base.Update();
            switch (currentState.CurrentState)
            {
                case EnemyStates.NPCSStateMachine.Guard:
                    {
                        if (distance < 2f && !player.GetComponent<Player.PlayerController>().isDisguised)
                        {
                            playerFound = true;
                            currentState.CurrentState = EnemyStates.NPCSStateMachine.Chase;
                        }

                        if (isHit)
                        {
                            HitReaction();
                        }

                        Guard();
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

                        if (isHit)
                            HitReaction();

                        Return();
                        break;
                    }

                default:
                    currentState.CurrentState = EnemyStates.NPCSStateMachine.Guard;
                    break;
            }
        }

        public override void Reset()
        {
            base.Reset();
            currentState.CurrentState = EnemyStates.NPCSStateMachine.Guard;
            reset = false;

        }

        private void ChasePlayer()
        {
            currentSpeed = runSpeed;
            FollowPlayer();

            if (playerFound && !hitFront && !hitRight && !hitLeft && !hitUp)
                LostTimer();
        }

        private void Return()
        {
            currentSpeed = walkSpeed;
            agent.isStopped = false;
            agent.destination = startPos;

            if (agent.remainingDistance < returnGuardDistance)
                currentState.CurrentState = EnemyStates.NPCSStateMachine.Guard;
        }

        private void Guard()
        {
            if (turned)
                newTurnAngle = turnAngle;
            else
                newTurnAngle = -turnAngle * 2;

            turnTime += Time.deltaTime * rotationSpeed;
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, Mathf.LerpAngle(currentRot, defaultRot + newTurnAngle, turnTime), transform.localEulerAngles.z);
            currentRot = transform.localEulerAngles.y;
            turnTime = 0;
        }

        private void TurnTimer()
        {
            turned = !turned;
        }
    }
}
