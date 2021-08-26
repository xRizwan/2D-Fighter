using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterHandler
{
    public int playerNumber;
    public KeyCode attackKey;
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
        healthManager = GetComponent<HealthManager>();
    }

    void Start()
    {

        StartGame();
    }

    void Update()
    {
        if (healthManager.health <= 0) return;

        if (Input.GetKeyDown(attackKey))
            should_attack = true;
        else
            should_attack = false;

        horizontal = Input.GetAxis("Horizontal" + playerNumber);
        vertical = Input.GetAxis("Vertical" + playerNumber);
    }

    void FixedUpdate()
    {
        if (healthManager.health <= 0) return;
        
        // If attack animations are playing, don't move.
        AnimatorStateInfo animState = animator.GetCurrentAnimatorStateInfo(0);
        if (!(animState.IsName(ATTACK1) || animState.IsName(ATTACK2) || animState.IsName(TRANSITION1) || animState.IsName(TRANSITION2) || healthManager.is_dazed))
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
        if (is_grounded && vertical > 0 && !healthManager.is_dazed)
        {
            animator.SetBool(JUMPING, true);
            base.Jump();
        }
    }

    public override void CheckForGround()
    {
        base.CheckForGround();
        
        if (is_grounded)
            animator.SetBool(JUMPING, false);
    }

    // Displays the radius of the attack range(circle), when the character is selected in the editor
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}