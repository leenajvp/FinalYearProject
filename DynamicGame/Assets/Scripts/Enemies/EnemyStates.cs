using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
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

        [SerializeField] private EnemyData data;

        [Header("Patroling targets")]
        [SerializeField] private Transform[] targets;
        private int destinationTarget = 0;

        [Header("PlayerDetection Settings")]
        [SerializeField] private GameObject player;
        [Tooltip("The guard will detect if player enters this radius and begin raycast.")]
        public float detectionRadius = 10f;
        [Tooltip("Raycast distance when player is detected.")]
        [SerializeField] private float detectionDistance = 10f;

        private float currentSpeed;
        private RaycastHit hit;
        public EnemyState CurrentState;
        private NavMeshAgent agent;

        private void Start()
        {
            gameObject.transform.position = targets[0].position;
            agent = GetComponent<NavMeshAgent>();
            agent.autoBraking = false;
            currentSpeed = data.walkingSpeed;
        }

        void Update()
        {
            agent.speed = currentSpeed;



            switch (CurrentState)
            {
                case EnemyState.Patrol:

                    PlayerDetection();
                    currentSpeed = data.walkingSpeed;
                    agent.isStopped = false;

                    if (!agent.pathPending && agent.remainingDistance < 0.5f)
                        Patrolling();

                    break;

                case EnemyState.PlayerSeen:

                    ShootPlayer();
                    currentSpeed = data.runningSpeed;

                    break;

                case EnemyState.ShootPlayer:

                    ShootPlayer();
                    currentSpeed = data.runningSpeed;

                    break;

                case EnemyState.AlertOthers:
                    break;

                default:

                    CurrentState = EnemyState.Patrol;

                    break;
            }
        }

        void PlayerDetection()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);

            foreach (var col in colliders)
            {
                IPlayer player = col.gameObject.GetComponent<IPlayer>();

                if (player != null)
                {
                    if (Physics.Raycast(transform.position, transform.forward, out hit, detectionDistance))
                    {
                        Debug.DrawRay(transform.position, transform.forward * 50);
                        IPlayer playerHit = hit.collider.gameObject.GetComponent<IPlayer>();

                        if (hit.collider == null)
                        {
                            return;
                        }

                        else if (playerHit != null)
                        {
                            Debug.Log("Guard detected player");
                        }
                    }
                }
            }
        }

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

            if (Vector3.Distance(transform.position, player.transform.position) <= data.shootDistance)
            {
                CurrentState = EnemyState.ShootPlayer;
            }

            if (Vector3.Distance(transform.position, player.transform.position) >= data.lostDistance)
            {
                CurrentState = EnemyState.Patrol;
            }
        }
    }
}
