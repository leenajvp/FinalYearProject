using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(KeyboardControls), typeof(Animator), typeof(PlayerMovement))]

public class PlayerAnimations : MonoBehaviour
{
    private PlayerMovement playerMove;
    private Animator animState;
    private KeyboardControls playerControls;
    private static readonly int currenAnim = Animator.StringToHash("AnimState");
    private static readonly int kickAnim = Animator.StringToHash("Kick");
    private static readonly int attackAnim = Animator.StringToHash("Attack");
    private static readonly int isMoving = Animator.StringToHash("isMoving");
    private float lastInteraction = 5.0f;


    void Awake()
    {
        animState = GetComponent<Animator>();
        playerControls = GetComponent<KeyboardControls>();
        playerMove = GetComponent<PlayerMovement>();
        animState.SetBool("isDead", false);
    }

    private void Start()
    {
        lastInteraction = Time.time;

    }


    void Update()
    {
        DisableMovement();
        MoveForwardAnimation();
        MoveBackwardsdAnimation();
        KickAnimation();
        AttackAnimation();
        DefeatAnimation();
    }

    public void MoveForwardAnimation()
    {
        if (Input.GetKey(playerControls.forward))
        {
            animState.SetBool(isMoving, true);
            animState.SetInteger(currenAnim, 1);

            if (Input.GetKey(playerControls.run))
            {
                animState.SetInteger(currenAnim, 2);
            }

            if (Input.GetKey(playerControls.crouch))
            {
                animState.SetInteger(currenAnim, 3);
            }

        }

        if (Input.GetKeyUp(playerControls.forward))
        {
            animState.SetInteger(currenAnim, 0);
            animState.SetBool(isMoving, false);
        }
    }

    public void MoveBackwardsdAnimation()
    {
        if (Input.GetKey(playerControls.backward))
        {
            animState.SetInteger(currenAnim, 3);

            if (Input.GetKey(playerControls.run))
            {
                animState.SetInteger(currenAnim, 4);
            }

            if (Input.GetKeyUp(playerControls.run))
            {
                animState.SetInteger(currenAnim, 3);
            }
        }

        if (Input.GetKeyUp(playerControls.backward))
        {
            animState.SetInteger(currenAnim, 0);
        }
    }

    public void AttackAnimation()
    {
        if (Time.time > lastInteraction + 1)
        {
            if (Input.GetKey(playerControls.attack))
            {
                lastInteraction = Time.time;
                animState.SetBool(attackAnim, true);
            }
        }

        if (Input.GetKeyUp(playerControls.attack))
        {
            animState.SetBool(attackAnim, false);
        }
    }

    public void KickAnimation()
    {
        if (Time.time > lastInteraction + 1)
        {
            if (Input.GetKey(playerControls.kick))
            {
                lastInteraction = Time.time;
                animState.SetBool(kickAnim, true);
            }
        }

        if (Input.GetKeyUp(playerControls.kick))
        {
            animState.SetBool(kickAnim, false);
        }
    }

    private void DisableMovement() 
    {
        //var checkKick = animState.GetCurrentAnimatorStateInfo(0).IsName("Kick");
        //var checkBigHit = animState.GetCurrentAnimatorStateInfo(0).IsName("BigHit");
        //var checkSpin = animState.GetCurrentAnimatorStateInfo(0).IsName("SpinHit");

        //if (checkBigHit || checkSpin || checkKick)
        //{
        //    playerMove.isStopped = true;
        //}

        //else
        //{
        //    playerMove.isStopped = false;
        //}
    }

    public void DefeatAnimation()
    {
        if (playerMove.isDead == true)
        {
            animState.SetBool("isDead", true);
        }
    }
}
