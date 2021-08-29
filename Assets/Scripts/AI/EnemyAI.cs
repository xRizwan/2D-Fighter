using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : CharacterHandler
{
    public string char_name = "ShieldDroid";
    // the amount of seconds after which the AI can attack
    public float attackInterval = 7.0f;

    // the farthest the AI can go from it's starting position
    public float moveRange = 4.0f;

    // makes AI move left or right (-1 or 1);
    private int move = 0;

    // saves the starting position so it can be used as a reference to drive AI movement
    private Vector3 startPos;

    // state checking
    private bool is_attacking;
    private bool resting = false;

    // for timers
    private float restTime = 2;
    private float timeToStop;
    private float timeTillAttack;

    // enemy bump interactions
    public int bumpDamage = 10;
    public float bumpSpeed = 50.0f;

    public float attackRangeLeft;
    public float attackRangeRight;

    public float attackDelay = 0.5f;

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
        if (healthManager.is_dead || healthManager.is_dazed) {Stop(); return; }
        
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
        if (healthManager.is_dead) return;

        timeTillAttack += Time.deltaTime;
        if (timeTillAttack >= attackInterval)
        {
            is_attacking = true;
            Stop();
            animator.SetTrigger("AttackOne");
            Attack(attackDelay);
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName(char_name + "_Attack"))
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
        if (!healthManager.is_dazed && !healthManager.is_dead)
        {
            Vector3 line_start_position = attackPoint.position;
            Vector3 line_end_position = attackPoint.position;

            if (m_facing_right) {
                line_start_position.x -= attackRangeLeft;
                line_end_position.x += attackRangeRight;
            } else {
                line_start_position.x -= attackRangeRight;
                line_end_position.x += attackRangeLeft;
            }

            RaycastHit2D[] hitEnemies = Physics2D.LinecastAll(line_start_position, line_end_position, enemyLayers);
            foreach(RaycastHit2D enemy in hitEnemies)
            {
                enemy.transform.gameObject.GetComponent<HealthManager>().TakeDamage(damageValue);
            }
        }
    }

    public void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        
        Vector3 line_start_position = attackPoint.position;
        Vector3 line_end_position = attackPoint.position;

        if (m_facing_right) {
            line_start_position.x -= attackRangeLeft;
            line_end_position.x += attackRangeRight;
        } else {
            line_start_position.x -= attackRangeRight;
            line_end_position.x += attackRangeLeft;
        }

        Gizmos.DrawLine(line_start_position, line_end_position);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            collision.gameObject.GetComponent<HealthManager>().TakeDamage(bumpDamage);
        }
    }
}
