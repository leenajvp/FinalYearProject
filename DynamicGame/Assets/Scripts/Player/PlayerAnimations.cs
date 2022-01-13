using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(KeyboardControls), typeof(Animator), typeof(PlayerMovement))]

public class PlayerAnimations : MonoBehaviour
{
    private PlayerMovement playerMove;
    private Animator animState;
    private KeyboardControls playerControls;

    void Awake()
    {
        animState = GetComponent<Animator>();
        playerControls = GetComponent<KeyboardControls>();
        playerMove = GetComponent<PlayerMovement>();
        animState.SetBool("isDead", false);
    }


    void Update()
    {
        // Animations: Animstate 1 = rIdle, 2 = attack, 3 = bigAttack, 4 = Kick, walk = 5, slowRun = 6, fast run = 7, walk back = 9, run back = 10 , Bool = isDead

        DisableMovement();
        MoveForwardAnimation();
        MoveBackwardsdAnimation();
        KickAnimation();
        SpinHitAnimation();
        BigHitAnimation();
        DefeatAnimation();
    }

    public void MoveForwardAnimation()
    {
        if (Input.GetKey(playerControls.forward))
        {
            animState.SetInteger("AnimState", 5);

            if (Input.GetKey(playerControls.run))
            {
                animState.SetInteger("AnimState", 6);
            }

            if (Input.GetKey(playerControls.stealth))
            {
                animState.SetInteger("AnimState", 7);
            }

        }

        if (Input.GetKeyUp(playerControls.forward))
        {
            animState.SetInteger("AnimState", 0);
        }
    }

    public void MoveBackwardsdAnimation()
    {
        if (Input.GetKey(playerControls.backward))
        {
            animState.SetInteger("AnimState", 9);

            if (Input.GetKey(playerControls.run))
            {
                animState.SetInteger("AnimState", 10);
            }

            if (Input.GetKeyUp(playerControls.run))
            {
                animState.SetInteger("AnimState", 9);
            }
        }

        if (Input.GetKeyUp(playerControls.backward))
        {
            animState.SetInteger("AnimState", 0);
        }
    }

    public void SpinHitAnimation()
    {
        if (Input.GetKeyDown(playerControls.basicAttack))
        {
            animState.SetBool("SpinHit", true);
        }

        if (Input.GetKeyUp(playerControls.basicAttack))
        {
            animState.SetBool("SpinHit", false);
        }
    }

    public void BigHitAnimation()
    {

        if (Input.GetKeyDown(playerControls.bigAttack))
        {
            animState.SetBool("BigHit", true);
        }

        if (Input.GetKeyUp(playerControls.bigAttack))
        {
            animState.SetBool("BigHit", false);
        }
    }

    public void KickAnimation()
    {
        if (Input.GetKeyDown(playerControls.kick))
        {
            
            animState.SetBool("Kick", true);
        }

        if (Input.GetKeyUp(playerControls.kick))
        {
            animState.SetBool("Kick", false);
        }
    }

    private void DisableMovement() 
    {
        var checkKick = animState.GetCurrentAnimatorStateInfo(0).IsName("Kick");
        var checkBigHit = animState.GetCurrentAnimatorStateInfo(0).IsName("BigHit");
        var checkSpin = animState.GetCurrentAnimatorStateInfo(0).IsName("SpinHit");

        if (checkBigHit || checkSpin || checkKick)
        {
            playerMove.isStopped = true;
        }

        else
        {
            playerMove.isStopped = false;
        }
    }

    public void DefeatAnimation()
    {
        if (playerMove.isDead== true)
        {
            animState.SetBool("isDead", true);
        }
    }
}
