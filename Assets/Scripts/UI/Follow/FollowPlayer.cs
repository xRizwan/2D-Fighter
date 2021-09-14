using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Vector3 current;
    public Transform player;
    public float smoothTime;
    public float offset = 7.0f;
    public float leftOffsetLimit = -0.9f;

    void FixedUpdate()
    {
        // handles player flipping (i.e looking left or right)
        if (player.localScale.x < 0 && offset > 0) offset = -offset;
        else if (player.localScale.x > 0 && offset < 0) offset = -offset;

        // getting target position
        Vector3 target = transform.position;
        float targetX = player.position.x - target.x;
        target.x += targetX + offset;

        // limiting target x position
        if (target.x <= leftOffsetLimit) target.x = leftOffsetLimit;

        // applying new position
        Vector3 newPos = Vector3.SmoothDamp(transform.position, target, ref current, smoothTime);
        transform.position = newPos;
    }
}
