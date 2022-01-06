using UnityEngine;
using UnityEngine.AI;
using System;
using System.Collections;

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
    Transform[] targets;
    private int destinationTarget = 0;
    private NavMeshAgent agent; 
    private Animator animState;
    public bool sitDown;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        
        animState = GetComponent<Animator>();
    }

    private void Update()
    {

        switch (currentSate)
        {
            case EnemyStates.Idle:
                Idle();
                break;

            case EnemyStates.Patrol:
                Patrolling();
                break;

            default:
                currentSate = EnemyStates.Patrol;
                break;
              
        }
    }

    void Idle()
    {
        agent.isStopped = true;

        if (sitDown)
        {
            animState.SetInteger("AnimState", 1);
        }

        else
        {
            animState.SetInteger("AnimState", 0);
        }

    }

    void Patrolling()
    {
        agent.isStopped = false;
        animState.SetInteger("AnimState", 3);

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            agent.SetDestination(targets[destinationTarget].position);
            
            destinationTarget = (destinationTarget + 1) % targets.Length;
            StartCoroutine(StopTimer());
        }
    }

    private void OnDrawGizmos()
    {
        for (var i = 1; i < targets.Length; i++)
        {
            Debug.DrawLine(targets[i - 1].transform.position, targets[i].transform.position);
        }
    }

    private IEnumerator StopTimer()
    {
        currentSate=EnemyStates.Idle;

        yield return new WaitForSeconds (10);

        currentSate = EnemyStates.Patrol;
    }
}
