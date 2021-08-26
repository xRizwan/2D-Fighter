using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : CharacterHandler
{
    // the amount of seconds after which the AI can attack
    public float attackInterval = 7.0f;

    // the farthest the AI can go from it's starting position
    public float moveRange = 4.0f;

    // makes AI move left or right (-1 or 1);
    int move = 0;

    // saves the starting position so it can be used as a reference to drive AI movement
    Vector3 startPos;

    // state checking
    bool is_attacking;
    bool resting = false;

    // for timers
    float restTime = 2;
    float timeToStop;
    float timeTillAttack;

    // for calculating the range of attack
    Vector3 line_start_position;
    Vector3 line_end_position;

    void Start()
    {
        is_attacking = false;
        startPos = transform.position;
        timeToStop = Time.deltaTime;
        timeTillAttack = Time.deltaTime;
        StartGame();
        SetRandomMoveDirection();

        // calculating attack range
        if (attackPoint == null) return;
        line_start_position = attackPoint.position;
        line_end_position = attackPoint.position;
        
        line_start_position.x -= attackRange;
        line_end_position.x += attackRange;
    }

    void FixedUpdate()
    {
        CountDownAttack();
        
        Rest();
        Move(move);
    }

    public override void Move(float horizontal)
    {
        if (!resting && !is_attacking)
        {
            HandleMoveConstraints();
            animator.SetFloat("Speed", Mathf.Abs(move));
            base.Move(horizontal);
        } else {
            animator.SetFloat("Speed", 0);
        }
    }

    private void HandleMoveConstraints()
    {
        if (transform.position.x >= startPos.x + moveRange && move == 1) {
            Stop();
            resting = true;
            move = -1;
        }
        else if(transform.position.x <= startPos.x - moveRange && move == -1) {
            Stop();
            resting = true;
            move = 1;
        };
    }

    private void SetRandomMoveDirection()
    {
        int result = Random.Range(0, 2);
        if (result == 0) move = -1;
        else move = 1;
    }

    private void Rest()
    {
        if (resting && !is_attacking)
        {
            timeToStop += Time.deltaTime;
            if (timeToStop >= restTime)
            {
                resting = false;
                timeToStop = 0;
            }
        }
    }

    private void CountDownAttack()
    {
        timeTillAttack += Time.deltaTime;
        if (timeTillAttack >= attackInterval)
        {
            is_attacking = true;
            Stop();
            animator.SetTrigger("AttackOne");
            Attack(0.5f);
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("ShieldDroid_Attack"))
        {
            is_attacking = true;
            timeTillAttack = 0;
            timeToStop = 0;
        } else {
            is_attacking = false;
        }
    }

    protected override void DealDamage()
    {
        if (!is_dazed)
        {
            RaycastHit2D[] hitEnemies = Physics2D.LinecastAll(line_start_position, line_end_position, enemyLayers);
            foreach(RaycastHit2D enemy in hitEnemies)
            {
                enemy.transform.gameObject.GetComponent<Player>().TakeDamage(damageValue);
            }
        }
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(line_start_position, line_end_position);
    }
}
