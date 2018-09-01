using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour {

    [Space]
    [Header("Melee settings")]
    public GameObject originOfAttack;
    public float xRange;
    public float yRange;
    public float meleeDelay;
    float nextMeleeDelay;
    public float meleeDamage;
    public LayerMask whatToHit;
    public GameObject attackSprite;
    public float dashspeed;
    bool isMeleeAttacking;
    public AudioClip MeleeSoundClip;

    // Not for Inspector:
    Vector2 myPos;
    Vector2 direction;

    private void FixedUpdate() {
        if (isMeleeAttacking) {
            AudioSource.PlayClipAtPoint(MeleeSoundClip, transform.position);
            attackSprite.GetComponent<Animator>().SetTrigger("isAttacking");
            Collider2D[] targets = Physics2D.OverlapBoxAll(originOfAttack.transform.position, new Vector2(xRange, yRange), 0, whatToHit);
            GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x * dashspeed, GetComponent<Rigidbody2D>().velocity.y);
            for (int i = 0; i < targets.Length; i++) {
                GetComponent<PlayerHealth>().LifeSteal(10);
                targets[i].GetComponent<Health>().TakeMeleeDamage(meleeDamage);
            }
            isMeleeAttacking = false;
            nextMeleeDelay = meleeDelay;
        }
    }

    private void Update() {
        Vector2 target = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        myPos = transform.position;
        direction = target - new Vector2(transform.position.x, transform.position.y);
        direction.Normalize();

        if (direction.x > 0) {
            attackSprite.GetComponent<SpriteRenderer>().flipX = false;
            originOfAttack.transform.position = this.transform.position + new Vector3(1.4f, 0, 0);
        }
        else {
            attackSprite.GetComponent<SpriteRenderer>().flipX = true;
            originOfAttack.transform.position = this.transform.position + new Vector3(-1.4f, 0, 0);
        }


        if (nextMeleeDelay <= 0) {
            if (Input.GetButtonDown("Fire2")) {
                isMeleeAttacking = true;
            }
        }
        else {
            nextMeleeDelay -= Time.deltaTime;
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(originOfAttack.transform.position, new Vector3(xRange, yRange, 1));
    }
}
