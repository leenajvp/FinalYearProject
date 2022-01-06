using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardControls : MonoBehaviour
{
    [Header("ControlScrip")]
    public KeyCode forward = KeyCode.W;
    public KeyCode backward = KeyCode.S;
    public KeyCode run = KeyCode.LeftShift;
    public KeyCode runFast = KeyCode.LeftControl;
    public KeyCode right = KeyCode.D;
    public KeyCode left = KeyCode.A;
    public KeyCode basicAttack = KeyCode.Q;
    public KeyCode kick = KeyCode.E;
    public KeyCode bigAttack = KeyCode.Tab;

    [Header("PlayerControllerScript")]
    [SerializeField]
    MonoBehaviour target;

    IPlayerControls playerCharacter;

    void Start()
    { 
      playerCharacter = target as IPlayerControls;
    }

    void Update()
    {
        if (Input.GetKey(forward)) playerCharacter.Forward();
        if (Input.GetKey(backward)) playerCharacter.Backward();
        if (Input.GetKeyDown(run) && Input.GetKey(forward)) 
        { 
            playerCharacter.isJogging = true;
        }
        if (Input.GetKeyUp(run))
        {
            playerCharacter.isJogging = false;
        }

        if (Input.GetKeyDown(runFast) && Input.GetKey(forward))
        {
            playerCharacter.isRunning = true;
        }
        if (Input.GetKeyUp(runFast))
        {
            playerCharacter.isRunning = false;
        }

        if (Input.GetKey(right)) playerCharacter.TurnRight();
        if (Input.GetKey(left)) playerCharacter.TurnLeft();
        if (Input.GetKeyDown(basicAttack)) playerCharacter.BasicAttack();
        if (Input.GetKeyDown(kick)) playerCharacter.Kick();
        if (Input.GetKeyDown(bigAttack)) playerCharacter.StrongAttack();
    }
}
