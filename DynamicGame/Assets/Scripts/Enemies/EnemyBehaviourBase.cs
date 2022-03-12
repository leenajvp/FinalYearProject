using Bullets;
using Player;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    public class EnemyBehaviourBase : MonoBehaviour
    {
        public EnemyData data;

        [Header("PlayerDetection Settings")]
        [SerializeField] protected GameObject player;
        private bool playerDisguised;
        [Tooltip("The guard will detect if player enters this radius and begin raycast.")]
        public float detectionRadius;
        [Tooltip("Raycast distance when player is detected.")]
        public float detectionDistance;
       // protected Collider[] colliders;
        public bool playerNear = false;
        public bool playerFound = false;

        [Header("Shooting")]
        [Tooltip("Position to shoot bullet from")]
        [SerializeField] protected Transform shootPoint;
        [Tooltip("Object pool for bullets")]
        [SerializeField] protected ObjectPool bulletPool;
        [SerializeField] protected ParticleSystem muzzleFlash;
        [SerializeField] protected ParticleSystem impactEffect;
        public float shootTimer = 0f;
        protected float distance;
        protected Vector3 playerPos;
        protected float currentSpeed;
        protected NavMeshAgent agent;
        protected RaycastHit hit;
        [HideInInspector] public bool isHit = false;

        public bool hitFront;
        public bool hitRight;
        public bool hitLeft;

        public float timer = 0;

        protected virtual void Start()
        {
            timer = 0;
            name = data.enemyName;
            agent = GetComponent<NavMeshAgent>();
            agent.autoBraking = false;
            currentSpeed = data.walkingSpeed;

            if (bulletPool == null)
                bulletPool = FindObjectOfType<ObjectPool>();

            if (player == null)
                player = FindObjectOfType<PlayerController>().gameObject;

            playerDisguised = player.GetComponent<PlayerController>().isDisguised;
        }

        protected virtual void Update()
        {
            Raycast();
            playerPos = player.transform.position;
            distance = Vector3.Distance(transform.position, playerPos);
            detectionDistance = data.detectionRayDistance;
            detectionRadius = data.detectionRadius;

            if (distance < data.detectionRadius)
                playerNear = true;

            else
                playerNear = false;
        }

        protected virtual void Raycast()
        {
            if (playerNear && !player.GetComponent<PlayerController>().isDisguised)
            {
                LostTimerI();

                if (Physics.Raycast(transform.position, transform.forward, out hit, detectionDistance))
                {
                    CheckFront();
                }

                if (Physics.Raycast(transform.position, transform.forward - transform.right, out hit, detectionDistance))
                {
                    Debug.DrawRay(transform.position, transform.forward - transform.right, Color.red, detectionDistance);
                    CheckLeft();
                }

                if (Physics.Raycast(transform.position, transform.forward + transform.right, out hit, detectionDistance))
                {
                    Debug.DrawRay(transform.position, transform.forward + transform.right, Color.red, detectionDistance);
                    CheckRight();
                }
            }
        }


        protected virtual void CheckFront()
        {
            IPlayer playerHit = hit.collider.gameObject.GetComponent<IPlayer>();

            if (playerHit != null)
            {
                hitFront = true;
            }

            else
                hitFront = false;
        }

        protected virtual void CheckLeft()
        {
            IPlayer playerHit = hit.collider.gameObject.GetComponent<IPlayer>();

            if (playerHit != null)
            {
                hitLeft = true;
            }

            else
                hitLeft = false;
        }

        protected virtual void CheckRight()
        {
            IPlayer playerHit = hit.collider.gameObject.GetComponent<IPlayer>();

            if (playerHit != null)
            {
                hitRight = true;
            }

            else
                hitRight = false;
        }

        protected void LostTimerI()
        {
            if (!hitFront && !hitRight && !hitLeft)
            {
                timer += 1 * Time.deltaTime;

                if (timer > 20)
                    playerFound = false;
            }

            else
            {
                timer = 0;
                playerFound = true;
            }
                

        }

        protected virtual void FollowPlayer()
        {
            currentSpeed = data.runningSpeed;

            agent.isStopped = true;
            Quaternion lookRotation = Quaternion.LookRotation((player.transform.position - transform.position).normalized); 
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 5f * Time.deltaTime); // Maintain rotation towards player

            if (distance <= data.shootDistance && distance >= data.retreatDistance)
            {
                agent.isStopped = true;
                ShootPlayer();
            }

            // Maintain distance
            else if (distance <= data.retreatDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, -data.walkingSpeed * Time.deltaTime);
            }

            // Return to default state if player is lost
            else if (distance >= data.lostDistance)
            {
                playerFound = false;
            }

            else
            {
                agent.isStopped = false;
                agent.destination = player.transform.position;
            }
        }

        protected virtual void ShootPlayer()
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
                    var newImpact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(newImpact, 0.6f);
                }

                else
                {
                    bulletController.target = transform.position + transform.forward * 25;
                    bulletController.hit = false;
                }
            }
        }

        // if other npcs detect player go towards
        //protected virtual void AlertMoreEnemies()
        //{
        //    foreach (var col in colliders)
        //    {
        //        PatrollingEnemyBehaviour enemies = col.gameObject.GetComponent<PatrollingEnemyBehaviour>();
        //        //enemies.CurrentState = EnemyState.PlayerSeen;
        //    }
        //}
    }
}
