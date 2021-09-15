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
        level_started = false;
        should_attack = true;
    }

    void Update()
    {
        NextMoveCountDown();

        if (go_to_next_move) {
            should_attack = true;
            go_to_next_move = false;
        }

        if (isChaining) {
            timeCount += Time.deltaTime;

            if (timeCount >= attackDelays[currentChain - 1]) {
                isChaining = false;
                timeCount = 0;
            }
        }
        if (reached_destination && !isChaining) InitiateAttack();
    }

    void FixedUpdate()
    {
        if (!reached_destination && should_attack && !should_cast) {
            FollowPlayer();
        }
    }

    protected override void InitiateAttack()
    {

        if (can_attack && should_attack)
        {
            Debug.Log(currentChain + "   :   " + attackStates[currentChain]);
            FacePlayer();
            animator.SetTrigger(attackStates[currentChain]);
            m_rb.AddForce(Vector2.right * (m_facing_right ? 1 : -1 ) * pushEffectSpeed, ForceMode2D.Impulse);
            // Attack(0.4f);

            isChaining = true;
            HandleChainAttacks();
        }
    }

    void HandleChainAttacks() {
        if (currentChain >= attackStates.Length - 1) {
            currentChain = 0;
            ResetState();
        }
        else currentChain += 1;
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
