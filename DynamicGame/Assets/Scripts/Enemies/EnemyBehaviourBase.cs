using Bullets;
using Player;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    [RequireComponent(typeof(SphereCollider))]
    public class EnemyBehaviourBase : MonoBehaviour
    {
        public EnemyData data;

        [Header("PlayerDetection Settings")]
        [SerializeField] protected GameObject player;
        [Tooltip("The guard will detect if player enters this radius and begin raycast.")]
        public float detectionRadius = 20f;
        [Tooltip("Raycast distance when player is detected.")]
        [SerializeField] protected float detectionDistance = 10f;
        protected Collider[] colliders;
        public bool playerNear = false;
        public bool playerFound = false;

        protected SphereCollider collider;

        [Header("Shooting")]
        [Tooltip("Position to shoot bullet from")]
        [SerializeField] protected Transform shootPoint;
        [Tooltip("Object pool for bullets")]
        [SerializeField] protected ObjectPool bulletPool;
        [SerializeField] protected ParticleSystem muzzleFlash;
        [SerializeField] protected ParticleSystem impactEffect;
        protected float shootTimer = 0f;
        protected float distance;
        protected Vector3 playerPos;
        protected float currentSpeed;
        protected NavMeshAgent agent;
        protected RaycastHit hit;


        protected virtual void Start()
        {
            name = data.enemyName;
            colliders = Physics.OverlapSphere(transform.position, detectionRadius);
            agent = GetComponent<NavMeshAgent>();
            agent.autoBraking = false;
            currentSpeed = data.walkingSpeed;

            if (bulletPool == null)
                bulletPool = FindObjectOfType<ObjectPool>();

            if (player == null)
                player = FindObjectOfType<PlayerController>().gameObject;

        }

        protected virtual void Update()
        {
           // collider.radius = data.detectionRadius;

             playerPos = player.transform.position;
             distance = Vector3.Distance(transform.position, playerPos);

            if (distance < detectionRadius)
                playerNear = true;

            else
                playerNear = false;
        }

        protected void Raycast()
        {
            if (playerNear)
            {
                if (Physics.Raycast(transform.position, transform.forward, out hit, detectionDistance))
                {
                    CheckRaycastHit();
                }

                if (Physics.Raycast(transform.position, transform.forward - transform.right, out hit, detectionDistance))
                {
                    Debug.DrawRay(transform.position, transform.forward - transform.right, Color.red, detectionDistance);
                    CheckRaycastHit();
                }

                if (Physics.Raycast(transform.position, transform.forward + transform.right, out hit, detectionDistance))
                {
                    Debug.DrawRay(transform.position, transform.forward + transform.right, Color.red, detectionDistance);
                    CheckRaycastHit();
                }
            }
        }

        protected virtual void CheckRaycastHit()
        {
            IPlayer playerHit = hit.collider.gameObject.GetComponent<IPlayer>();

            if (playerHit != null)
            {
                playerFound = true;
            }

            else
                playerFound = false;
        }

        protected virtual void FollowPlayer()
        {
            currentSpeed = data.runningSpeed;

            //Get distance to player and maintain rotation towards
            agent.isStopped = true;
           // var distance = Vector3.Distance(player.transform.position,transform.position);
            Quaternion lookRotation = Quaternion.LookRotation((player.transform.position - transform.position).normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 5f * Time.deltaTime);


            if (distance <= data.shootDistance && distance >= data.retreatDistance)
            {
                agent.isStopped = true;
                ShootPlayer();
            }

            ////Maintain distance
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
        protected virtual void AlertMoreEnemies()
        {
            foreach (var col in colliders)
            {
                PatrollingEnemyBehaviour enemies = col.gameObject.GetComponent<PatrollingEnemyBehaviour>();
                //enemies.CurrentState = EnemyState.PlayerSeen;
            }
        }


        //protected virtual void Raycasting()
        //{
        //    foreach (var col in colliders)
        //    {
        //        IPlayer player = col.gameObject.GetComponent<IPlayer>();

        //        if (player != null)
        //        {
        //            Debug.Log("playerin col");

        //            if (Physics.Raycast(transform.position, transform.forward, out hit, detectionDistance))
        //            {
        //                CheckRaycastHit();
        //            }

        //            if (Physics.Raycast(transform.position, transform.forward - transform.right, out hit, detectionDistance))
        //            {
        //                Debug.DrawRay(transform.position, transform.forward - transform.right, Color.red, detectionDistance);
        //                CheckRaycastHit();
        //            }

        //            if (Physics.Raycast(transform.position, transform.forward + transform.right, out hit, detectionDistance))
        //            {
        //                Debug.DrawRay(transform.position, transform.forward + transform.right, Color.red, detectionDistance);
        //                CheckRaycastHit();
        //            }
        //        }
        //    }
        //}
    }
}
