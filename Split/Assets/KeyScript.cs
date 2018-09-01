using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour {

    public GameObject doorToOpen;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            Destroy(doorToOpen);
            Destroy(gameObject);
        }
    }
}
