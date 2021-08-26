using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : CharacterHandler
{
    public float attackInterval = 7.0f;
    public int moveRange;
    private int move = 0;
    private Vector3 startPos;
    private Vector3 velocity;
    private bool resting = false;
    private float restTime = 2;
    private float timeToStop;
    private float timeTillAttack;
    private bool is_attacking;

    void Start()
    {
        is_attacking = false;
        startPos = transform.position;
        timeToStop = Time.deltaTime;
        timeTillAttack = Time.deltaTime;
        StartGame();
        SetRandomMoveDirection();
    }

    // Update is called once per frame
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

    public void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        
        Vector3 line_start_position = attackPoint.position;
        Vector3 line_end_position = attackPoint.position;
        line_start_position.x -= attackRange;
        line_end_position.x += attackRange;

        Gizmos.DrawLine(line_start_position, line_end_position);
    }

    private void CountDownAttack()
    {
        timeTillAttack += Time.deltaTime;
        if (timeTillAttack >= attackInterval)
        {
            is_attacking = true;
            Stop();
            animator.SetTrigger("AttackOne");
            // Attack()
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
}
