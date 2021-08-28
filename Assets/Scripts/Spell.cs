using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    public LayerMask enemyLayers;
    public Transform attackPoint;
    public float attackRangeX;
    public float attackRangeY;
    public int damageValue;

    void Start()
    {
        Attack(1f);
    }

    public void Attack(float delay)
    {
        Invoke("DealDamage", delay);
        Destroy(gameObject, 2.4f);
    }

    // Attacks nearby enemy by creating a circle to check if they're within it's bounds
    protected virtual void DealDamage()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position, new Vector2(attackRangeX, attackRangeY), enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            if (enemy.gameObject.CompareTag("Player")) 
            {
                enemy.GetComponent<HealthManager>().TakeDamage(damageValue);
            }
        }
    }

    public void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawCube(attackPoint.position, new Vector3(attackRangeX, attackRangeY, 0));
    }
}
