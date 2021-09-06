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
    
    protected HealthManager healthManager;
    
    public bool should_attack;
    public float attackRange = 0.5f;

    public bool m_facing_right = true;
    protected bool is_grounded = true;
    [SerializeField] protected float m_jump_force = 8;
    [SerializeField] protected int damageValue = 10;
    public bool can_attack = false;

    public virtual void StartGame()
    {
        m_rb = GetComponent<Rigidbody2D>();
        healthManager = GetComponent<HealthManager>();
    }

    public virtual void CheckForGround()
    {
        is_grounded = Physics2D.OverlapCircle(ground_check.position, 0.2f, ground_layer);
    }

    // all functions have been made virtual so animations can be added differently for each different character
    // by overriding them.

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

    // Stops player movement
    public virtual void Stop()
    {
        m_rb.velocity = Vector3.zero;
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

    // Flips the character left/right
    protected void Flip() {
        m_facing_right = !m_facing_right;

        Vector3 _scale = transform.localScale;
        _scale.x *= -1;
        transform.localScale = _scale;
    }

    protected void ResetVelocityY()
    {
        m_rb.velocity = new Vector2(m_rb.velocity.x, 0f);
    }
}
