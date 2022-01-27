using UnityEngine;
using UnityEngine.AI;
using Player;

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

    [Header("Patrol")]
    [SerializeField] private Transform[] targets;

    [Header("PlayerDetection")]
    [SerializeField] private GameObject player;
    [SerializeField] private float catchedDistance = 10;
    [SerializeField] private float lostDistance = 20f;
    [SerializeField] private float stopDistance = 3f;
    [SerializeField] private GameObject gameBoss;

    private NavMeshAgent agent;
    private float defaultSpeed => agent.speed = 3.5f;
    private float runningSpeed => agent.speed = 7;
    private float speed;

    private int destinationTarget = 0;
    private RaycastHit hit;

    private PlayerController playerSript;

    public EnemyState CurrentState;

    // DDA STUFF

    private GameObject head;
    private string headName;



    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerSript = player.GetComponent<PlayerController>();
        agent.autoBraking = false;


    }

    void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        Debug.DrawRay(transform.position, transform.forward * 50);

        switch (CurrentState)
        {
            case EnemyState.Patrol:

                RaycastCheck();
                speed = defaultSpeed;
                agent.isStopped = false;

                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                    Patrolling();

                break;

            case EnemyState.PlayerSeen:
                
                GoToPlayer();
                speed = runningSpeed;

                break;

            case EnemyState.ShootPlayer:

                GoToPlayer();
                speed = runningSpeed;

                break;

            case EnemyState.AlertOthers:
                break;

            default:

                CurrentState = EnemyState.Patrol;

                break;
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other == gameObject.tag=="bullet")
    //    {

    //    }
    //    // if head gets hit add headshot
    //    // if any part gets hit add hit
    //}

    void RaycastCheck()
    {
        if (hit.collider == null)
        {
            return;
        }

        else
        {

            if (hit.collider.gameObject == player)
            {
                CurrentState = EnemyState.PlayerSeen;
            }
        }
    }

    //patrol when player is lost
    void Patrolling()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.blue;

        agent.destination = targets[destinationTarget].position;
        destinationTarget = (destinationTarget + 1) % targets.Length;
    }


    // if other npcs detect player go towards
    void PLayerBeenSnitched()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.yellow;

        if (gameBoss.GetComponent<GameManager>().playerDetected == true)
        {
            agent.SetDestination(player.transform.position);
        }

        else
        {
            CurrentState = EnemyState.Patrol;
        }
    }


    //Move towards player and stop when on shooting distance
    void GoToPlayer()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.red;
        agent.SetDestination(player.transform.position);

       

        if (Vector3.Distance(transform.position, player.transform.position) <= catchedDistance)
        {
            CurrentState = EnemyState.ShootPlayer;
        }

        if (Vector3.Distance(transform.position, player.transform.position) >= lostDistance)
        {
            CurrentState = EnemyState.Patrol;
        }
    }
}
