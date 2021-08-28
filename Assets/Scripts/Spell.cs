using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRangeX;
    public float attackRangeY;
    public int damageValue;
    public LayerMask enemyLayers;

    void Start()
    {
        Attack(1f);
    }

    public void Attack(float delay)
    {
        Invoke("DealDamage", delay);
    }

    // Attacks nearby enemy by creating a circle to check if they're within it's bounds
    protected virtual void DealDamage()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position, new Vector2(attackRangeX, attackRangeY), enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<HealthManager>().TakeDamage(damageValue);
        }
    }

    public void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        
        Vector3 line_start_position = attackPoint.position;
        Vector3 line_end_position = attackPoint.position;

        // line_start_position.y += attackRangeUp;
        // line_end_position.y -= attackRangeDown;

        // Gizmos.DrawLine(line_start_position, line_end_position);
        // Gizmos.Draw(attackPoint.position, attackRangeDown);
        Gizmos.DrawCube(attackPoint.position, new Vector3(attackRangeX, attackRangeY, 0));
    }
}
