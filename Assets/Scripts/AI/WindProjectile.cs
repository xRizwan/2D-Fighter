using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindProjectile : MonoBehaviour
{
    public float moveSpeed = 10;
    public bool isFacingRight;
    public float destoryAfterSeconds = 5f;
    private float _timer;

    void Start()
    {
        if (isFacingRight) {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    void Update()
    {
        // destroy after a specified amount of seconds have passed
        _timer += Time.deltaTime;
        if (_timer >= destoryAfterSeconds) Destroy(gameObject);

        transform.Translate(Vector3.left * (isFacingRight ? -1 : 1) * moveSpeed * Time.deltaTime);
    }
}
