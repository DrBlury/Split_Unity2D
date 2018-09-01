using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour {

    [Header("Movement settings")]
    public bool canMove = true;
    public float moveSpeed = 3f;
    public float bulletSpeed = 3f;
    public float runTowardsDistance = 5f;
    public float noticeDistance = 15f;
    public float backOffDistance= 0f;
    public float knockbackPower;

    // Not for Inspector:
    GameObject player;
    Transform myTrans;
    Transform playerTrans;
    Quaternion rotation;
    Vector2 direction;

    [Space]
    [Header("Shooting settings")]
    
    public GameObject bulletPrefab;
    public float shootingDelayMin;
    public float shootingDelayMax;
    float shootingDelay;
    float nextshotDelay;
    public Vector3 shootingOffset;

    [Space]
    [Header("Melee settings")]
    public bool isMelee;
    public GameObject originOfAttack;
    public float xRange;
    public float yRange;
    public float meleeDelay;
    float nextMeleeDelay;
    public float meleeDamage;
    public LayerMask whatToHit;
    SpriteRenderer mySpriteRenderer;



    // Use this for initialization
    void Start () {
        shootingDelay = Random.Range(shootingDelayMin, shootingDelayMax);
        player = GameObject.Find("Player");
        mySpriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (player != null) {
            myTrans = this.transform;
            playerTrans = player.transform;
            direction = playerTrans.position - new Vector3(transform.position.x, transform.position.y);
            direction.Normalize();

            if (GetComponent<Health>().nowDizzyTime <= 0) {
                if (Vector2.Distance(myTrans.position, playerTrans.position) < noticeDistance) {
                    // Keepdistance should be higher than backOffDistance...

                    if (canMove) {
                        if (Vector2.Distance(myTrans.position, playerTrans.position) > runTowardsDistance) {
                            transform.position = Vector2.MoveTowards(myTrans.position, playerTrans.position, moveSpeed * Time.deltaTime);
                        }
                        else if (Vector2.Distance(myTrans.position, playerTrans.position) < backOffDistance) {
                            transform.position = Vector2.MoveTowards(myTrans.position, playerTrans.position, -moveSpeed * Time.deltaTime);
                        }
                    }
                    if (direction.x > 0) {
                        mySpriteRenderer.flipX = true;
                    }
                    else {
                        mySpriteRenderer.flipX = false;
                    }

                    if (!isMelee) {
                        if (nextshotDelay <= 0) {
                            rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
                            GameObject projectile = (GameObject)Instantiate(bulletPrefab, transform.position + shootingOffset, rotation);
                            projectile.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
                            nextshotDelay = shootingDelay;
                            Destroy(projectile, 5.5f);
                        }
                        else {
                            nextshotDelay -= Time.deltaTime;
                        }
                    }
                    else {
                        if (nextMeleeDelay <= 0) {
                            Collider2D[] targets = Physics2D.OverlapBoxAll(originOfAttack.transform.position, new Vector2(xRange, yRange), 0, whatToHit);

                            for (int i = 0; i < targets.Length; i++) {
                                Debug.Log(targets[i]);
                                targets[i].GetComponent<PlayerHealth>().TakeDamage(meleeDamage);
                            }
                            nextMeleeDelay = meleeDelay;
                        }
                        else {
                            nextMeleeDelay -= Time.deltaTime;
                        }
                    }
                }
            }
        }       
    }

    public void TakeKnockback() {
        GetComponent<Rigidbody2D>().velocity = -direction * knockbackPower;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(originOfAttack.transform.position, new Vector3(xRange, yRange, 1));
    }
}
