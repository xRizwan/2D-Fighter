using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : CharacterHandler
{
    // prefab of the spell object
    public GameObject castingPrefab;
    public Transform player;

    // how close to the player to go while following the player
    public float stop_x_from_player = 4.0f;

    // delay before next move can be decided
    public float attack_delay = 3.0f;
    protected float next_move_time;
    protected bool go_to_next_move;
    protected bool should_cast;
    protected bool level_started;

    // if boss has reached close enough to the player
    protected bool reached_destination;

    // Start is called before the first frame update
    void Start()
    {
        StartGame();
        ResetState();
        level_started = true;
        next_move_time = attack_delay;
        

        StartCoroutine("StartStage");
    }

    // Update is called once per frame
    void Update()
    {
        if (level_started) return;

        NextMoveCountDown();

        if (healthManager.is_dazed || healthManager.is_dead) return;

        // decide whether to go to perform melee attack or cast magic
        DecideNextMove();
        
        // casting and attacking
        InitiateAttack();
        Cast();
    }

    void FixedUpdate()
    {
        if (!reached_destination && should_attack && !should_cast) {
            FollowPlayer();
        }
    }

    public override void Move(float horizontal)
    {
        animator.SetFloat("Speed", Mathf.Abs(horizontal));
        base.Move(horizontal);
    }

    public override void Stop()
    {
        animator.SetFloat("Speed", 0);
        base.Stop();
    }

    protected void NextMoveCountDown()
    {
        // if we can't go to the next move
        // countup till we can go to the next move
        if (!go_to_next_move) {
            next_move_time += Time.deltaTime;
            if(next_move_time >= attack_delay) go_to_next_move = true;
        }
    }

    // chooses whether to cast magic or attack player with melee weapon
    protected void DecideNextMove()
    {
        if (go_to_next_move && !should_attack && !should_cast && !can_attack) {
            int result = Random.Range(0, 3);

            if (result == 2) should_cast = true;
            else should_attack = true;
        }
    }

    // follows player
    protected void FollowPlayer()
    {
        bool is_to_right = IsToRight();
        bool is_to_left = IsToLeft();

        if (is_to_right) Move(1);
        else if(is_to_left) Move(-1);

        // check whether player has reached close enough to the player to initiate melee attack.
        bool reached_right = (is_to_right) && (transform.position.x >= player.position.x - stop_x_from_player);
        bool reached_left = (is_to_left) && (transform.position.x <= player.position.x + stop_x_from_player);

        if (reached_right || reached_left)
        {
            Stop();
            can_attack = true;
            should_attack = true;
            reached_destination = true;
        }
    }

    // attacks player if allowed
    protected virtual void InitiateAttack()
    {
        if (can_attack && should_attack)
        {
            animator.SetTrigger("AttackOne");
            Attack(0.4f);
            should_attack = false;
        }
    }

    protected override void DealDamage()
    {
        Debug.Log("Attacking");
        base.DealDamage();
        ResetState();
    }

    // for casting magic spell
    protected void Cast()
    {
        if (should_cast) {
            if (IsToRight() && !m_facing_right) Flip();
            else if (IsToLeft() && m_facing_right) Flip();

            animator.SetTrigger("Casting");
            
            Invoke("InstantiateSpell", 0.1f);
            ResetState();
        }
    }

    // Instantiates a new spell object
    protected void InstantiateSpell()
    {
        Vector2 prefab_position = player.transform.position;
        prefab_position.y += 2;
        Instantiate(castingPrefab, prefab_position, castingPrefab.transform.rotation);
    }

    // resets boss state;
    protected virtual void ResetState(){
        reached_destination = false;
        go_to_next_move = false;
        should_attack = false;
        should_cast = false;
        can_attack = false;
        next_move_time = 0;
    }

    // Displays the radius of the attack range(circle), when the character is selected in the editor
    protected void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    protected IEnumerator StartStage()
    {
        yield return new WaitForSeconds(2);
        level_started = false;
    }

    protected bool IsToRight()
    {
        return player.position.x > transform.position.x;
    }

    protected bool IsToLeft()
    {
        return player.position.x < transform.position.x;
    }

}
