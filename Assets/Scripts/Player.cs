using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterHandler
{
    public int playerNumber;
    private float horizontal;
    private float vertical;

    // Animation parameters
    const string SPEED = "Speed";
    const string JUMPING = "Is_Jumping";

    // Animation names
    const string ATTACK1 = "Player_Attack_1";
    const string ATTACK2 = "Player_Attack_2";
    const string TRANSITION1 = "Player_Attack_Transition_0";
    const string TRANSITION2 = "Player_Attack_Transition_1";

    void Awake()
    {
        health = 100;
    }

    void Update()
    {
        if (health <= 0) return;

        if (Input.GetKeyDown(attackKey))
        {
            should_attack = true;
        } else {
            should_attack = false;
        }

        horizontal = Input.GetAxis("Horizontal" + playerNumber);
        vertical = Input.GetAxis("Vertical" + playerNumber);
    }

    void FixedUpdate()
    {
        if (health <= 0) return;
        
        // If attack animations are playing, don't move.
        AnimatorStateInfo animState = animator.GetCurrentAnimatorStateInfo(0);
        if (!(animState.IsName(ATTACK1) || animState.IsName(ATTACK2) || animState.IsName(TRANSITION1) || animState.IsName(TRANSITION2) || is_dazed))
        {
            Move(horizontal);
            Jump();

            CheckForGround();
        } else {
            Stop();
        }
    }

    public override void Move(float horizontal)
    {
        animator.SetFloat(SPEED, Mathf.Abs(horizontal));
        base.Move(horizontal);
    }

    public override void Jump()
    {
        if (is_grounded && vertical > 0 && !is_dazed)
        {
            base.Jump();
            animator.SetBool(JUMPING, true);
        }
    }

    public override void CheckForGround()
    {
        base.CheckForGround();
        
        if (is_grounded)
            animator.SetBool(JUMPING, false);
    }

}