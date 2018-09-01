using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomJump : MonoBehaviour {
    public float jumpTimeMin;
    public float jumpTimeMax;
    public float jumpTime;
    public bool jump;
    Rigidbody2D rb;
    public float jumpForce;
    public float fallMultiplier = 2.5f;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        jumpTime = Random.Range(jumpTimeMin, jumpTimeMax);
    }

    // Update is called once per frame
    void Update () {
        jumpTime -= Time.deltaTime;
        if (jumpTime <= 0) {
            jump = true;
        }

        if (rb.velocity.y < 0) {
            rb.gravityScale = fallMultiplier;
        } else {
            rb.gravityScale = 1f;
        }
    }

    private void FixedUpdate() {
        if (jump == true) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpTime = Random.Range(jumpTimeMin, jumpTimeMax);
            jump = false;
        }
    }
}
