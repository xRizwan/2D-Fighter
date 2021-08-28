using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    private Animator animator;
    public HealthBar healthBar;
    public int health = 100;
    public bool is_dead = false;
    public bool is_dazed = false;
    public float dazeDuration = 0.5f;
    public string characterName = "Player";
    private Rigidbody2D rigidBody;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (healthBar) healthBar.SetMaxHealth(health);
    }

    public void TakeDamage(int damageToTake)
    {
        if (is_dead) return;
        
        is_dazed = true;
        Hurt();
        Invoke("Undaze", dazeDuration);
        health -= damageToTake;
        if (healthBar) healthBar.SetHealth(health);
        if (health <= 0) {
            IsDead();
            return;
        }
    }
    public virtual void IsDead()
    {
        animator.SetTrigger("Is_Dead");
        is_dead = true;
        GetComponent<CapsuleCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().Sleep();
    }

    private void Undaze()
    {
        is_dazed = false;
    }

    public virtual void Hurt()
    {
        animator.SetTrigger("Hurt");
        // animator.Play(characterName + "_Hurt");
    }
}
