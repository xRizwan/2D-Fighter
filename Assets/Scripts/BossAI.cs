using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : CharacterHandler
{
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        Debug.Log(player.position.x);
        Debug.Log(player.position.x == transform.position.x);

        if (player.position.x > transform.position.x) Move(1);
        else if(player.position.x < transform.position.x) Move(-1);

    }

    public override void Move(float horizontal)
    {
        animator.SetFloat("Speed", horizontal);
        base.Move(horizontal);
    }

    // Displays the radius of the attack range(circle), when the character is selected in the editor
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
