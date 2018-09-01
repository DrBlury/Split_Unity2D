using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour {

    public float damage;
    public GameObject explosionPrefab;
    public bool isPlayerBullet;

    private void OnTriggerEnter2D(Collider2D collision) {
        var hit = collision.gameObject;
        if (isPlayerBullet) {
            if (hit.CompareTag("Enemy")) {
                StartCoroutine(Camera.main.GetComponent<CameraShake>().Shake(0.35f, 0.05f));
                GameObject explosion = (GameObject)Instantiate(explosionPrefab, transform.position + new Vector3(0, 1, -0.15f), Quaternion.identity);
                Destroy(explosion, 3f);
                var health = hit.GetComponent<Health>();
                if (health != null) {
                    health.TakeDamage(damage);
                    Destroy(gameObject);
                }
            }
            else if (!hit.CompareTag("Player")) {
                StartCoroutine(Camera.main.GetComponent<CameraShake>().Shake(0.35f, 0.05f));
                GameObject explosion = (GameObject)Instantiate(explosionPrefab, transform.position + new Vector3(0, 1, -0.15f), Quaternion.identity);
                Destroy(explosion, 3f);
                Destroy(gameObject);
            }
        }
        else {
            if (hit.CompareTag("Player")) {
                hit.GetComponent<PlayerHealth>().TakeDamage(damage);
                GameObject explosion = (GameObject)Instantiate(explosionPrefab, transform.position + new Vector3(0, 1, -0.15f), Quaternion.identity);
                Destroy(explosion, 3f);
                Destroy(gameObject);
            } else {
                GameObject explosion = (GameObject)Instantiate(explosionPrefab, transform.position + new Vector3(0, 1, -0.15f), Quaternion.identity);
                Destroy(explosion, 3f);
                Destroy(gameObject);
            }
        }
    }
}
