using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanAI : BossAI
{
    public string[] attackStates = new string[0];
    public float[] attackDelays = new float[0];
    public float pushEffectSpeed;
    private bool isChaining;
    private int currentChain;
    private float timeCount;

    void Start()
    {
        StartGame();
        level_started = true;

        StartCoroutine("StartStage");
    }

    void Update()
    {

        if (level_started) return;

        NextMoveCountDown();

        if (healthManager.is_dazed || healthManager.is_dead) return;

        if (go_to_next_move) {
            DecideNextMove();
        }

        if (isChaining) {
            timeCount += Time.deltaTime;

            if (timeCount >= attackDelays[currentChain - 1]) {
                isChaining = false;
                timeCount = 0;
            }
        }
        if (reached_destination && !isChaining) InitiateAttack();
        Cast();
    }

    void FixedUpdate()
    {
        if (!reached_destination && should_attack && !should_cast) {
            FollowPlayer();
        }
    }

    protected override void InstantiateSpell()
    {
        Vector2 prefab_position = transform.position;
        GameObject cast = Instantiate(castingPrefab, prefab_position, castingPrefab.transform.rotation);
        cast.GetComponent<WindProjectile>().isFacingRight = m_facing_right;
    }

    protected override void InitiateAttack()
    {

        if (should_attack)
        {
            FacePlayer();

            animator.SetTrigger(attackStates[currentChain]);
            m_rb.AddForce(Vector2.right * (m_facing_right ? 1 : -1 ) * pushEffectSpeed, ForceMode2D.Impulse);
            
            float attackAfter = Mathf.Clamp(attackDelays[currentChain] / 2, 0.3f, 2);
            Attack(attackAfter);

            isChaining = true;
            HandleChainAttacks();
        }
    }

    void HandleChainAttacks() {
        if (currentChain >= attackStates.Length - 1) {
            currentChain = 0;
            ResetState();
            can_attack = true;
            Invoke("ChanceToGoToNextMove", 0.5f);
        }
        else {
            currentChain += 1;
            can_attack = true;
        };
    }

    void ChanceToGoToNextMove()
    {
        int random = Random.Range(0, 5);
        if (random >= 3) {
            should_cast = true;
        }
    }

    protected override void DealDamage()
    {
        if (!healthManager.is_dazed && can_attack && !healthManager.is_dead)
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

            foreach(Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<HealthManager>().TakeDamage(damageValue);
            }
        }

        can_attack = false;
    }

    protected override void ResetState()
    {
        base.ResetState();
        currentChain = 0;
        should_attack = false;
        isChaining = false;
    }

    protected void FacePlayer()
    {
        if (IsToLeft()) Move(-0.01f);
        else Move(0.01f);
    }
}
