using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemies;

public class ExplorationNpc : PatrollingEnemyBehaviour
{
    protected override void Patrolling()
    {
        agent.destination = targets[1].position;

        if (isHit)
        {
            timer = 0;
            CurrentState = EnemyState.ShootPlayer;
        }
    }
}
