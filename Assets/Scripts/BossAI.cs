using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : CharacterHandler
{
    public Transform player;
    private bool reached_destination;
    public float stop_x_from_player = 4.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (!reached_destination) {
            FollowPlayer();
        }
        InitiateAttack();
    }

    private void FollowPlayer()
    {
        bool is_to_right = player.position.x > transform.position.x;
        bool is_to_left = player.position.x < transform.position.x;

        if (is_to_right) Move(1);
        else if(is_to_left) Move(-1);


        // check whether player has reached close enough to the player to initiate melee attack.
        bool reached_right = (is_to_right) && (transform.position.x >= player.position.x - stop_x_from_player);
        bool reached_left = (is_to_left) && (transform.position.x <= player.position.x + stop_x_from_player);

        if (reached_right || reached_left)
        {
            Stop();
            can_attack = true;
            reached_destination = true;
        }

        // if (is_to_right && transform.position.x >= player.position.x - 4){
        //     Stop();
        //     can_attack = true;
        //     reached_destination = true;
        // }
        // else if (is_to_left && transform.position.x <= player.position.x + 4){
        //     Stop();
        // }
    }

    public override void Move(float horizontal)
    {
        animator.SetFloat("Speed", Mathf.Abs(horizontal));
        base.Move(horizontal);
    }

    private void InitiateAttack()
    {
        if (can_attack)
        {
            animator.SetTrigger("AttackOne");
            // Attack(0.2f);
            // Debug.Log("Attacking");
            can_attack = false;
        }
    }

    private void ResetState(){
        can_attack = false;
        reached_destination = false;
    }

    public override void Stop()
    {
        animator.SetFloat("Speed", 0);
        base.Stop();
    }

    // Displays the radius of the attack range(circle), when the character is selected in the editor
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
