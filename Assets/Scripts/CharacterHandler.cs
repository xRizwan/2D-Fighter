using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterHandler : MonoBehaviour
{   
    [Range(0, 0.5f)] [SerializeField] protected float m_MovementSmoothing = 0.5f;
    [SerializeField] protected float m_speed = 8;
    protected Rigidbody2D m_rb;
    protected Vector3 m_Velocity = Vector3.zero;
    public LayerMask enemyLayers;
    public Transform attackPoint;
    public LayerMask ground_layer;
    public Transform ground_check;
    public Animator animator;
    public KeyCode attackKey;
    
    public bool should_attack;
    public bool is_dead;
    public float attackRange = 0.5f;

    [SerializeField] protected bool m_facing_right = true;
    protected bool is_grounded = true;
    [SerializeField] protected float m_jump_force = 8;
    public int health { get; protected set; } = 100;
    [SerializeField] private int damageValue = 10;
    protected bool is_dazed = false;
    [SerializeField] protected float dazeDuration = 0.5f;

    public HealthBar healthBar;
    public bool can_attack = false;

    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
        healthBar.SetMaxHealth(health);
    }

    public virtual void CheckForGround()
    {
        is_grounded = Physics2D.OverlapCircle(ground_check.position, 0.2f, ground_layer);
    }

    // all functions have been made virtual so animations can be added differently for each different character

    // Moves the character forward or backward
    public virtual void Move(float horizontal) {
        
        // Flipping the player depending on the value of horizontal i.e if -1 the character should face left and vice versa
        if (horizontal > 0 && !m_facing_right) Flip();
        else if (horizontal < 0 && m_facing_right) Flip();

        // Creating a target velocity(of position) where the character will move to
        // and smoothning it
        Vector3 target_velocity = new Vector2(horizontal * m_speed, m_rb.velocity.y);
        m_rb.velocity = Vector3.SmoothDamp(m_rb.velocity, target_velocity, ref m_Velocity, m_MovementSmoothing);

    }

    // Adds an upward force to make the character jump
    public virtual void Jump() {
        m_rb.AddForce(new Vector2(0, m_jump_force), ForceMode2D.Impulse);
        is_grounded = false;
    }

    public void Attack(float delay)
    {
        Invoke("DealDamage", delay);
    }

    // Attacks nearby enemy by creating a circle to check if they're within it's bounds
    protected virtual void DealDamage()
    {
        if (!is_dazed && can_attack)
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

            foreach(Collider2D enemy in hitEnemies)
            {
                Debug.Log("Enemy Hit and took damage: " + damageValue);
                enemy.GetComponent<Player>().TakeDamage(damageValue);
            }
        }

        can_attack = false;
    }

    // Makes the character take damage
    public virtual void TakeDamage(int damageToTake)
    {
        if (is_dead) return;
        
        is_dazed = true;
        Hurt();
        Invoke("Undaze", dazeDuration);
        health -= damageToTake;
        healthBar.SetHealth(health);

        if (health <= 0) {
            IsDead();
        }
    }

    public virtual void IsDead()
    {
        Debug.Log("Player Dead");
        animator.SetTrigger("Is_Dead");
        is_dead = true;
        GetComponent<CapsuleCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().Sleep();
    }

    public virtual void Hurt()
    {
        animator.Play("Player_Hurt");
    }
    
    private void Undaze()
    {
        is_dazed = false;
    }

    // Flips the character left/right
    private void Flip() {
        m_facing_right = !m_facing_right;

        Vector3 _scale = transform.localScale;
        _scale.x *= -1;
        transform.localScale = _scale;
    }

    // Displays the radius of the attack range(circle), when the character is selected in the editor
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
