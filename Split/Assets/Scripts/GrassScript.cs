using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision) {
        var hit = collision.gameObject;
        if (hit.CompareTag("Enemy") || collision.CompareTag("Player")) {
            GetComponent<Animator>().SetBool("keepIdle", false);
            GetComponent<Animator>().SetTrigger("isShaking");
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        var hit = collision.gameObject;
        if (hit.CompareTag("Enemy") || collision.CompareTag("Player")) {
            GetComponent<Animator>().SetBool("keepIdle", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        var hit = collision.gameObject;
        if (hit.CompareTag("Enemy") || collision.CompareTag("Player")) {
            GetComponent<Animator>().SetBool("keepIdle", true);
        }
    }
}
