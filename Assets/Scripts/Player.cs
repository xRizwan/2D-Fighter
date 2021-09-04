using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : CharacterHandler
{
    // attack key
    public KeyCode attackKey;
    public int playerNumber;

    private float horizontal;
    private float vertical;

    // Animation parameters
    const string SPEED = "Speed";
    const string JUMPING = "Is_Jumping";

    // Animation names
    const string ATTACK1 = "Player_Attack_1";
    const string ATTACK2 = "Player_Attack_2";
    const string TRANSITION1 = "Player_Attack_Transition_0";
    const string TRANSITION2 = "Player_Attack_Transition_1";

    // for tilemap referencing and fall handling
    private Vector3 lastLandPos;
    private Tilemap tiles;

    void Start()
    {
        StartGame();
        // finding tilemap_base(ground tiles) if they exist
        if (GameObject.Find("Tilemap_Base"))
            tiles = GameObject.Find("Tilemap_Base").GetComponent<Tilemap>();
    }

    void Update()
    {
        if (healthManager.health <= 0) return;

        if (Input.GetKeyDown(attackKey))
            should_attack = true;
        else
            should_attack = false;

        horizontal = Input.GetAxis("Horizontal" + playerNumber);
        vertical = Input.GetAxis("Vertical" + playerNumber);
    }

    void FixedUpdate()
    {
        if (healthManager.health <= 0) return;
        
        // If attack animations are playing, don't move.
        AnimatorStateInfo animState = animator.GetCurrentAnimatorStateInfo(0);
        if (!(animState.IsName(ATTACK1) || animState.IsName(ATTACK2) || animState.IsName(TRANSITION1) || animState.IsName(TRANSITION2) || healthManager.is_dazed))
        {
            Move(horizontal);
            Jump();

            CheckForGround();
        } else {
            Stop();
        }
    }

    public override void Move(float horizontal)
    {
        animator.SetFloat(SPEED, Mathf.Abs(horizontal));
        base.Move(horizontal);
    }

    public override void Jump()
    {
        if (is_grounded && vertical > 0 && !healthManager.is_dazed)
        {
            animator.SetBool(JUMPING, true);
            base.Jump();
        }
    }

    public override void CheckForGround()
    {
        base.CheckForGround();
        
        // if is_grounded set current position as lastLandPos
        // used for handling falls and finding closest tiles
        if (is_grounded)
        {
            lastLandPos = transform.position;
            animator.SetBool(JUMPING, false);
        }
        else 
            animator.SetBool(JUMPING, true);
    }

    // Displays the radius of the attack range(circle), when the character is selected in the editor
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    // for checking for falls below ground
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Water"))
        {
            if (!tiles) return;
            GameManager.Instance.UpdateScore(-5);
            Invoke("LandToNearestTile", 0.5f);
        }
    }

    // checks against all tiles in the level and finds the tiles most nearest to player location
    void LandToNearestTile() {
        healthManager.TakeDamageNoAnim(10);

        foreach (var position in tiles.cellBounds.allPositionsWithin) {
            
            // checking if a tile exists at the position
            if (!tiles.HasTile(position)) {
                continue;
            }
            Vector3 vector_pos = position;
            bool foundTileLeft = vector_pos.x <= lastLandPos.x && vector_pos.x >= (lastLandPos.x - 5);
            bool foundTileRight = vector_pos.x >= lastLandPos.x && vector_pos.x <= (lastLandPos.x + 5);

            // if tile is found, move player to that tile
            if (foundTileLeft || foundTileRight) {
                vector_pos.x += 1;
                vector_pos.y = lastLandPos.y;
                if (!healthManager.is_dead) vector_pos.y += 10;
                transform.position = vector_pos;
                Stop();
                break;
            }
        }
    }

}