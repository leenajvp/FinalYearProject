using Bullets;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    [RequireComponent(typeof(NavMeshAgent))]

    public class EnemyBehaviour : MonoBehaviour
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

        [Header("Shooting")]
        [Tooltip("Position to shoot bullet from")]
        [SerializeField] private Transform shootPoint;
        [Tooltip("Object pool for bullets")]
        [SerializeField] private ObjectPool bulletPool;
        [SerializeField] private ParticleSystem muzzleFlash;
        [SerializeField] private ParticleSystem impactEffect;

        float shootTimer = 0f;

        private void Start()
        {
            name = data.enemyName;
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

                    FollowPlayer();
                    currentSpeed = data.runningSpeed;

                    break;

                case EnemyState.ShootPlayer:

                    FollowPlayer();
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
                        RaycastCheck();
                    }

                    if (Physics.Raycast(transform.position, transform.forward - transform.right, out hit, detectionDistance))
                    {
                        RaycastCheck();
                    }

                    if (Physics.Raycast(transform.position, transform.forward + transform.right, out hit, detectionDistance))
                    {
                        RaycastCheck();
                    }
                }
            }
        }

        private void RaycastCheck()
        {
            IPlayer playerHit = hit.collider.gameObject.GetComponent<IPlayer>();

            if (hit.collider == null)
            {
                return;
            }

            else if (playerHit != null)
            {
                CurrentState = EnemyState.PlayerSeen;
            }

            else
            {
                //look for player, get player rotation and rotate and move towards player, if player is not found for X sec give up and return patrol
            }
        }

        //Patrol between set points when player is not detected
        private void Patrolling()
        {
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
        private void FollowPlayer()
        {
            //Get distance to player and maintain rotation towards

            var distance = Vector3.Distance(transform.position, player.transform.position);
            Quaternion lookRotation = Quaternion.LookRotation((player.transform.position - transform.position).normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 5f * Time.deltaTime);

            if (distance <= data.shootDistance && distance >= data.retreatDistance)
            {
                agent.isStopped = true;
                Shoot();
            }

            //Maintain distance

            else if (distance <= data.retreatDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, -data.walkingSpeed * Time.deltaTime);
            }

            else if (distance >= data.lostDistance)
            {
                CurrentState = EnemyState.Patrol;
            }

            else
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, data.walkingSpeed * Time.deltaTime);
            }
        }

        private void Shoot()
        {
            if (Time.time < shootTimer + data.shootSpeed)
                return;

            muzzleFlash.Play();

            RaycastHit hit;
            GameObject newBullet = bulletPool.GetObject();
            newBullet.transform.position = shootPoint.position;
            newBullet.transform.rotation = shootPoint.rotation;
            newBullet.SetActive(true);
            BulletController bulletController = newBullet.GetComponent<BulletController>();
            shootTimer = Time.time;

            if (Physics.Raycast(transform.position, transform.forward, out hit, detectionDistance))
            {
                IPlayer playerHit = hit.collider.gameObject.GetComponent<IPlayer>();

                if (playerHit != null)
                {
                    bulletController.target = hit.point;
                    bulletController.hit = true;
                }

                else
                {
                    bulletController.target = transform.position + transform.forward * 25;
                    bulletController.hit = false;
                }
            }
        }
    }
}
