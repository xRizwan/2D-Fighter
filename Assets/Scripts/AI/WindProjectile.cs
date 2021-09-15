using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindProjectile : MonoBehaviour
{
    public float moveSpeed = 10;
    public bool isFacingRight;

    void Start()
    {
        if (isFacingRight) {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * (isFacingRight ? -1 : 1) * moveSpeed * Time.deltaTime);
    }
}
