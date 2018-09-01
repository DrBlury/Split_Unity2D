using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {
    public AudioClip clip;
    public string powerUpType;

    private void OnTriggerEnter2D(Collider2D collision) {
        var hit = collision.gameObject;
        if (hit.CompareTag("Player")) {
            hit.GetComponent<PlayerHealth>().UsePowerUp(powerUpType);
            AudioSource.PlayClipAtPoint(clip, transform.position);
            Destroy(gameObject);
        }
    }
}
