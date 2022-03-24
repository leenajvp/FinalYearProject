using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStates : MonoBehaviour
{
    public enum NPCSStateMachine { Guard, Patrol, Chase, Search, Return, Idle }
    public NPCSStateMachine CurrentState;
}
