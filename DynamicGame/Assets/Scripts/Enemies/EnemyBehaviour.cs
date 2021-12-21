using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]

public class EnemyBehaviour : MonoBehaviour
{
    public enum EnemyStates
    {
        Idle,
        Patrol
    }

    public EnemyStates currentSate;

    [SerializeField]
    Transform[]
        targets;
    private int destinationTarget = 0;
    private NavMeshAgent agent => GetComponent<NavMeshAgent>();
    private Animator animState;

    private void Start()
    {
        agent.autoBraking = false;
        animState = GetComponent<Animator>();
    }

    private void Update()
    {

        switch (currentSate)
        {
            case EnemyStates.Idle:
                break;

            case EnemyStates.Patrol:
                Patrolling();
                break;

            default:
                currentSate = EnemyStates.Patrol;
                break;
              
        }
    }

    void Patrolling()
    {
        animState.SetBool("isMoving", true);

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            agent.SetDestination(targets[destinationTarget].position);
            destinationTarget = (destinationTarget + 1) % targets.Length;
        }
    }

    private void OnDrawGizmos()
    {
        for (var i = 1; i < targets.Length; i++)
        {
            Debug.DrawLine(targets[i - 1].transform.position, targets[i].transform.position);
        }
    }
}
