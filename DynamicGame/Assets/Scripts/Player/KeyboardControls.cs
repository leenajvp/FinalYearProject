using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardControls : MonoBehaviour
{
    [Header("ControlScrip")]
    public KeyCode forward = KeyCode.W;
    public KeyCode backward = KeyCode.S;
    public KeyCode right = KeyCode.D;
    public KeyCode left = KeyCode.A;

    public KeyCode run = KeyCode.LeftShift;
    public KeyCode crouch = KeyCode.F;

    public KeyCode attack = KeyCode.Mouse0;
    public KeyCode kick = KeyCode.Mouse1;

    public KeyCode changeGun = KeyCode.Alpha1;

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
        if (Input.GetKey(right)) playerCharacter.TurnRight();
        if (Input.GetKey(left)) playerCharacter.TurnLeft();

        if (Input.GetKeyDown(run) && Input.GetKey(forward)) 
        { 
            playerCharacter.isCrawling = true;
        }
        if (Input.GetKeyUp(run))
        {
            playerCharacter.isCrawling = false;
        }

        if (Input.GetKeyDown(crouch) && Input.GetKey(forward))
        {
            playerCharacter.isRunning = true;
        }
        if (Input.GetKeyUp(crouch))
        {
            playerCharacter.isRunning = false;
        }

        if (Input.GetKeyDown(attack)) playerCharacter.WeaponAttack();
        if (Input.GetKeyDown(kick)) playerCharacter.Kick();
        if (Input.GetKeyDown(changeGun)) playerCharacter.ChangeGun();
    }
}
