using DDA;
using Player;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
namespace Enemies
{
    [RequireComponent(typeof(EnemyStates))]
    public class EnemyBehaviourBase : MonoBehaviour
    {
        public EnemyData data;
        public bool isBoss;

        [Header("PlayerDetection Settings")]
        [SerializeField] protected GameObject player;
        /*[HideInInspector]*/
        public float detectionRadius;
        /*[HideInInspector]*/
        public float detectionDistance;
        /*[HideInInspector]*/
        public float retreatDist;
        /*[HideInInspector]*/
        public float walkSpeed;
        /*[HideInInspector]*/
        public float runSpeed;
        public float rotSpeed;
        public bool hitFront, hitRight, hitLeft, hitUp;
        public bool playerNear = false;
        public bool playerFound = false;
        LayerMask layer_mask;
        private int layerMask;
        [Header("Shooting")]
        [Tooltip("Position to shoot bullet from")]
        [SerializeField] private GameObject hitEffect;
        [SerializeField] protected Transform shootPoint;
        /*[HideInInspector]*/
        public float shootFrequency;
        /*[HideInInspector]*/
        public float shootDist;
        /*[HideInInspector]*/
        public float bDamage;
        [Header("Shooting SFX")]
        [SerializeField] private ObjectPool impactPool;
        [SerializeField] protected ParticleSystem muzzleFlash;
        [SerializeField] protected ParticleSystem impactEffect;
        [SerializeField] protected float impactTimer = 0.5f;
        [HideInInspector] public float shootTimer = 0f;
        [SerializeField] protected DDAManager ddaManager;

        protected float timer = 0;
        protected float distance;
        protected Vector3 playerPos;
        protected float currentSpeed;
        protected NavMeshAgent agent;
        protected RaycastHit hit;
        public bool isHit = false;

        public Vector3 startPos;

        private EnemyHealth health;
        private EnemyPools ePool;
        protected EnemyStates currentState;
        public float searchTime = 10;


        protected bool reset = false;
        private PlayerInventory pInventory => FindObjectOfType<PlayerInventory>();

        protected void Awake()
        {
            if (impactPool == null)
                impactPool = GameObject.Find("SmallImpactPool").GetComponent<ObjectPool>();

            hitEffect = GameObject.Find("BloodShot");
            agent = GetComponent<NavMeshAgent>();
            currentState = GetComponent<EnemyStates>();
            startPos = transform.position;
        }

        protected virtual void Start()
        {
            layer_mask = LayerMask.GetMask("Player", "Default", "Environment", "Door", "Objects", "Ground");
            layerMask = layer_mask;
            timer = 0;
            name = data.enemyName;
            agent.autoBraking = true;
            currentSpeed = data.walkingSpeed;
            rotSpeed = data.rotationSpeed;
            shootFrequency = data.shootSpeed;
            retreatDist = data.retreatDistance;
            bDamage = data.bulletDamage;
            detectionDistance = data.detectionRayDistance;
            detectionRadius = data.detectionRadius;
            shootDist = data.shootDistance;
            runSpeed = data.runningSpeed;
            walkSpeed = data.walkingSpeed;

            health = GetComponent<EnemyHealth>();
            ePool = health.pool;

            if (player == null)
                player = FindObjectOfType<PlayerController>().gameObject;

            if (ddaManager == null)
                ddaManager = FindObjectOfType<DDAManager>();
        }

        private void FixedUpdate()
        {
            if (reset)
            {
                playerFound = false;
                searchTime = 0;
                StopAllCoroutines();
            }

        }

        protected virtual void Update()
        {
            agent.speed = currentSpeed;
            playerPos = player.transform.position;
            distance = Vector3.Distance(transform.position, playerPos);

            Raycast();

            if (playerFound)
                currentState.CurrentState = EnemyStates.NPCSStateMachine.Chase;

            if (distance < detectionRadius)
                playerNear = true;

            else
                playerNear = false;
        }

        protected virtual void Raycast()
        {
            if (hitFront || hitRight || hitLeft || hitUp)
            {
                searchTime = 5;
                playerFound = true;
                StartCoroutine(AlertOthersTimer(3));
            }


            if (playerNear && !player.GetComponent<PlayerController>().isDisguised)
            {
                if (Physics.Raycast(transform.position, transform.forward, out hit, detectionDistance))
                {
                    CheckFront();
                }

                if (Physics.Raycast(transform.position, transform.forward - transform.right, out hit, detectionDistance, layer_mask))
                {
                    // Debug.DrawRay(transform.position, transform.forward - transform.right, Color.red, detectionDistance);
                    CheckLeft();
                }

                if (Physics.Raycast(transform.position, transform.forward + transform.right, out hit, detectionDistance, layer_mask))
                {
                    // Debug.DrawRay(transform.position, transform.forward + transform.right, Color.red, detectionDistance);
                    CheckRight();
                }


                if (Physics.Raycast(transform.position, transform.forward + transform.up * 0.2f, out hit, detectionDistance, layer_mask))
                {

                    // Debug.DrawRay(transform.position, transform.forward + transform.up * 0.2f, Color.red, detectionDistance);
                    CheckUp();
                }
            }
        }

        protected virtual void CheckFront()
        {
            IPlayer playerHit = hit.collider.gameObject.GetComponent<IPlayer>();

            if (playerHit != null)
                hitFront = true;

            else
                hitFront = false;
        }

        protected virtual void CheckLeft()
        {
            IPlayer playerHit = hit.collider.gameObject.GetComponent<IPlayer>();

            if (playerHit != null)
                hitLeft = true;

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

        protected virtual void CheckUp()
        {
            IPlayer playerHit = hit.collider.gameObject.GetComponent<IPlayer>();

            if (playerHit != null)
            {
                hitUp = true;
            }

            else
                hitUp = false;
        }

        protected void LostTimer()
        {
            if (!playerNear)
                timer = 20;

            if (playerFound)
            {
                if (reset)
                    return;

                timer += 1 * Time.deltaTime;

                if (hitFront || hitRight || hitLeft || hitUp)
                {
                    timer = 0;
                    return;
                }

                else if (timer > searchTime && playerFound) // if player not found in X sec
                {
                    playerFound = false;
                    currentState.CurrentState = EnemyStates.NPCSStateMachine.Return;
                    timer = 0;
                }
            }
        }

        protected virtual void FollowPlayer()
        {
            currentSpeed = runSpeed;

            if (agent.isOnNavMesh)
            {
                Quaternion lookRotation = Quaternion.LookRotation((player.transform.position - transform.position).normalized);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotSpeed * Time.deltaTime); // Maintain rotation towards player

                if (hitFront || hitUp || hitLeft || hitRight)
                {


                    while (distance <= shootDist && distance >= retreatDist)
                    {
                        agent.isStopped = true;
                        ShootPlayer();
                        break;
                    }
                }

                while (distance >= retreatDist)
                {
                    agent.isStopped = false;
                    agent.destination = new Vector3(player.transform.position.x + retreatDist, transform.position.y, player.transform.position.z + retreatDist);
                    break;
                }

                while (distance < retreatDist) // Maintain distance
                {
                    agent.isStopped = false;
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, -walkSpeed * Time.deltaTime);

                    break;
                }
            }
        }

        protected virtual void ShootPlayer()
        {
            if (Time.time < shootTimer + shootFrequency)
                return;

            muzzleFlash.Play();
            RaycastHit hit;
            shootTimer = Time.time;

            if (Physics.Raycast(transform.position, transform.forward, out hit, detectionDistance))
            {
                PlayerHealth playerHit = hit.collider.gameObject.GetComponent<PlayerHealth>();

                if (playerHit != null)
                {
                    if (hitEffect != null)
                        hitEffect.SetActive(true);

                    playerHit.currentHealth -= bDamage;
                    ddaManager.currentPHits++;

                    GameObject newImpact = impactPool.GetObject();
                    newImpact.transform.position = hit.point;
                    newImpact.transform.rotation = Quaternion.LookRotation(hit.normal);
                    newImpact.SetActive(true);
                }
            }

            else if (Physics.Raycast(transform.position, transform.forward + transform.up * 0.2f, out hit, detectionDistance))
            {
                PlayerHealth playerHit = hit.collider.gameObject.GetComponent<PlayerHealth>();

                if (playerHit != null)
                {
                    hitEffect.SetActive(true);
                    playerHit.currentHealth -= bDamage;
                    ddaManager.currentPHits++;
                    GameObject newImpact = impactPool.GetObject();
                    newImpact.transform.position = hit.point;
                    newImpact.transform.rotation = Quaternion.LookRotation(hit.normal);
                    newImpact.SetActive(true);
                }
            }

            else return;
        }

        public virtual void Reset()
        {
            reset = true;
            hitFront = false;
            hitRight = false;
            hitLeft = false;
            hitUp = false;
            transform.position = startPos;
            playerFound = false;
            searchTime = 0;
            gameObject.GetComponent<Renderer>().material = data.defaultMaterial;

            if (isBoss)
            {
                IQuestItems keycard = health.objectToSpawn.GetComponent<IQuestItems>();
                pInventory.RemoveItem();
            }

            if (agent.isOnNavMesh && agent.isActiveAndEnabled)
            {
                agent.destination = startPos;
            }

        }

        protected IEnumerator AlertOthersTimer(int numberToAlert)
        {
            yield return new WaitForSeconds(2); // if player does not kill npc within 2 sec they will alert 3 others in same pool

            if (ePool != null)
            {
                foreach (Transform child in ePool.transform)
                {
                    for (int i = 0; i < ePool.originalCount; i++)
                    {
                        if (i < numberToAlert && i < ePool.originalCount)
                        {
                            child.GetComponent<EnemyBehaviourBase>().playerFound = true;
                            child.GetComponent<EnemyStates>().CurrentState = EnemyStates.NPCSStateMachine.Chase;
                        }
                    }
                }
            }
        }

        protected void HitReaction()
        {
            searchTime = 10;
            playerFound = true;
            currentState.CurrentState = EnemyStates.NPCSStateMachine.Chase;
            StartCoroutine(AlertOthersTimer(6));
            isHit = false;
        }
    }
}
