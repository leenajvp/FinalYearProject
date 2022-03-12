using System.Collections;
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
        private Vector3 guardPos;
        private float newTurnAngle;
        private float defaultRot;
        private float currentRot;
        private float turnTime = 0;
        public bool turned;

        private Transform t;

        public enum EnemyState
        {
            Guard,
            PlayerSeen,
            ReturnGuard,
            AlertOthers,
            ShootPlayer
        }

        public EnemyState CurrentState;

        protected override void Start()
        {
            t = transform;
            base.Start();
            guardPos = transform.position;
            defaultRot = transform.localEulerAngles.y;
            currentRot = transform.localEulerAngles.y;
            InvokeRepeating("TurnTimer", 0, 4);
        }

        protected override void Update()
        {
            base.Update();
            agent.speed = currentSpeed;

            switch (CurrentState)
            {
                case EnemyState.Guard:

                    Raycast();
                    Quarding();

                    if (playerFound)
                        CurrentState = EnemyState.PlayerSeen;

                    break;

                case EnemyState.PlayerSeen:

                    FollowPlayer();
                    currentSpeed = data.runningSpeed;

                    if (!playerFound)
                        CurrentState = EnemyState.ReturnGuard;

                    break;


                case EnemyState.ReturnGuard:

                    agent.speed = data.walkingSpeed;
                    agent.isStopped = false;
                    agent.destination = guardPos;

                    if (agent.remainingDistance < 0.5f)
                        CurrentState = EnemyState.Guard;



                    break;

                case EnemyState.ShootPlayer:

                    FollowPlayer();
                    currentSpeed = data.runningSpeed;

                    break;

                case EnemyState.AlertOthers:
                    // get enemies in radius and get them target the player
                    break;

                default:

                    CurrentState = EnemyState.Guard;

                    break;
            }
        }

        //Patrol between set points when player is not detected
        private void Quarding()
        {
            if (distance < detectionDistance)
            {
                if (turned)
                    newTurnAngle = turnAngle;
                else
                    newTurnAngle = -turnAngle * 2;

                turnTime += Time.deltaTime * rotationSpeed;
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, Mathf.LerpAngle(currentRot, defaultRot + newTurnAngle, turnTime), transform.localEulerAngles.z);
                currentRot = transform.localEulerAngles.y;
                turnTime = 0;
                if (isHit)
                {
                    StartCoroutine(ReactionTimer());
                }
                    
            }
        }

        private IEnumerator ReactionTimer()
        {
            yield return new WaitForSeconds(1);
            CurrentState = EnemyState.PlayerSeen;
            isHit = false;
        }


        private void TurnTimer()
        {
            turned = !turned;
        }
    }
}
