using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanAI : MonoBehaviour
{
    public Transform player;

    // how close to the player to go while following the player
    public float stop_x_from_player = 4.0f;

    // delay before next move can be decided
    public float attack_delay = 3.0f;
    float next_move_time;
    bool go_to_next_move;
    bool should_cast;
    bool level_started;

    // if AI has reached close enough to the player
    bool reached_destination;
}
