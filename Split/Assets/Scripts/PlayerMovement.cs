using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public CharacterController controller;
    public float speed;
    public float horizontalMove;
    bool jump = false;

    void Update() {
        horizontalMove = Input.GetAxisRaw("Horizontal") * speed;

        if (Input.GetButtonDown("Jump")) {
            jump = true;
        }
    }

    private void FixedUpdate() {
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
        jump = false;
    }
}
