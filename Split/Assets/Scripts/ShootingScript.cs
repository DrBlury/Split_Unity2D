using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingScript : MonoBehaviour {

    bool shootActive = false;
    public AudioClip ShootSoundClip;
    public float speed;
    public GameObject bullet;
    Vector2 myPos;
    Quaternion rotation;
    Vector2 direction;
    public int Shot;
    public bool canShoot = true;
    public float standardDelayInSeconds;
    public float delayInSeconds;
    public float projectileTimeToLive;

    private void Start() {
        delayInSeconds = standardDelayInSeconds;
    }

    // Update is called once per frame
    void Update () {
        if (Shot == 1) {
            Shot = 0;
        }

        if (Input.GetButton("Fire1")) {
            Vector2 target = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            myPos = transform.position;
            direction = target - new Vector2 (transform.position.x, transform.position.y);
            direction.Normalize();
            rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            shootActive = true;
        }
	}

    public void SetStandardDelayInSeconds() {
        delayInSeconds = standardDelayInSeconds;
    }

    private void FixedUpdate() {
        if (shootActive == true) {
            shoot(direction);
        }
    }

    public void shoot(Vector2 direction) {
        if (shootActive == true) {
            if (Shot == 0 && canShoot == true) {
                AudioSource.PlayClipAtPoint(ShootSoundClip, transform.position);
                GameObject projectile = (GameObject)Instantiate(bullet, myPos, rotation);
                shootActive = false;
                projectile.GetComponent<Rigidbody2D>().velocity = direction * speed;
                Shot = 1;
                canShoot = false;
                StartCoroutine(ShootDelay());
                Destroy(projectile, projectileTimeToLive);
            }
        }
    }

    IEnumerator ShootDelay() {
        yield return new WaitForSeconds(delayInSeconds);
        canShoot = true;
    }
}
