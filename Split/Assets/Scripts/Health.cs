using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Health : MonoBehaviour {
    [Header("General Settings")]
    public bool isABot;
    public bool canRegenerate;
    GameObject enemySpawner;

    [Header("Health Settings")]
    public GameObject HealthBarCanvasPrefab;
    public Vector3 healthbarOffset;
    public float maxHealth = 200;
    [Tooltip("Only necessary if it is not a bot. Bots do not regenerate health.")]
    public float healthRegenerationDelay = 5;
    public float healthRegenerationRate = 5;

    [Header("Shield Settings")]
    public float maxShield = 200;
    [Tooltip("Only necessary if it is not a bot. Bots do not regenerate health.")]
    public float shieldRegenerationDelay = 3;
    public float shieldRegenerationRate = 20;

    [Header("Drop Info")]
    public GameObject[] drops;

    [Header("Debug Info")]
    public int dropchance;
    public float nowDizzyTime;
    public float dizzyTime = 1f;
    bool givenPoints = false;
    public GameObject PointsManagerObject;
    public int killPoints;
    bool explosionIsSpawned = false;
    public GameObject explosionPrefab;
    public float camShakeDuration;
    public float camShakeMagnitude;
    [SerializeField] GameObject myHealthBar;
    [SerializeField] float currentHealth;
    [SerializeField] bool isRegenHealth;
    [SerializeField] float currentShield;
    [SerializeField] bool isRegenShield;
    [SerializeField] public static float timestamp = 0.0f;

    void Start() {
        var healthbar = Instantiate(HealthBarCanvasPrefab, this.transform.position + healthbarOffset, Quaternion.Euler(0, 0, 0));
        myHealthBar = healthbar;
        healthbar.GetComponent<HealthBarScript>().Initialize(this.gameObject, this.gameObject, new Vector3(0f, 1f, 0f));

        currentHealth = maxHealth;
        currentShield = maxShield;
    }

    public void Update() {
        if (nowDizzyTime > 0) {
            nowDizzyTime -= Time.deltaTime;
        }

        if (canRegenerate) {
            if (currentHealth != maxHealth && !isRegenHealth) {
                StartCoroutine(RegainHealthOverTime());
            }

            if (currentShield != maxShield && !isRegenShield) {
                StartCoroutine(RegainShieldOverTime());
            }
        }

        if (currentHealth < maxHealth || currentShield < maxShield) {
            myHealthBar.SetActive(true);
        } else {
            myHealthBar.SetActive(false);
        }

        if (currentHealth <= 0) {
            GetComponent<Animator>().SetBool("isDead", true);
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            if (isABot) {
                StartCoroutine(Camera.main.GetComponent<CameraShake>().Shake(camShakeDuration, camShakeMagnitude));
                if (!explosionIsSpawned) {
                    GameObject explosion = (GameObject)Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                    Destroy(explosion, 3f);
                    explosionIsSpawned = true;
                }

                GivePointsToPlayer();
                Destroy(gameObject, 0.5f);
            }

        }
        myHealthBar.GetComponent<HealthBarScript>().updateHealthShield(currentHealth, maxHealth, currentShield, maxShield);
    }

    private void GivePointsToPlayer() {
        if (givenPoints == false) {
            int drop100 = Random.Range(0, 100);
            if (drop100 < dropchance) {
                GameObject drop = (GameObject)Instantiate(drops[Random.Range(0, drops.Length - 1)], transform.position + new Vector3(0f, 1f), Quaternion.identity);
                drop.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 2);
            }

            GameObject.Find("PointsManager").GetComponent<PointsManager>().AddPoints(killPoints);
            givenPoints = true;
        }
    }

    public void TakeDamage(float damage) {
        GetComponent<Animator>().SetTrigger("Hit");
        timestamp = Time.time;
        if (currentShield >= damage) {
            currentShield -= damage;
            
        }
        else {
            damage = damage - currentShield;
            currentShield = 0;
            currentHealth -= damage;
            if (currentHealth >= 0) {
                GetComponent<Animator>().SetTrigger("Hit");
            }
        }
    }

    public void TakeMeleeDamage(float damage) {
        GetComponent<Animator>().SetTrigger("Hit");
        GetComponent<EnemyBehavior>().TakeKnockback();
        nowDizzyTime = dizzyTime;
        timestamp = Time.time;
        if (currentShield >= damage) {
            currentShield -= damage;
            
        }
        else {
            damage = damage - currentShield;
            currentShield = 0;
            currentHealth -= damage;
            if (currentHealth >= 0) {
                GetComponent<Animator>().SetTrigger("Hit");
            }
        }
    }

    private IEnumerator RegainHealthOverTime() {
        isRegenHealth = true;
        while (currentHealth < maxHealth && Time.time > (timestamp + healthRegenerationDelay)) {
            Healthregen();
            yield return new WaitForSeconds(1);
        }
        isRegenHealth = false;
    }

    private IEnumerator RegainShieldOverTime() {
        isRegenShield = true;
        while (currentShield < maxShield && Time.time > (timestamp + shieldRegenerationDelay)) {
            Shieldregen();
            yield return new WaitForSeconds(1);
        }
        isRegenShield = false;
    }

    public void Healthregen() {
        if (maxHealth - currentHealth < healthRegenerationRate) {
            currentHealth = maxHealth;
        }
        else {
            currentHealth += healthRegenerationRate;
        }
    }
    public void Shieldregen() {
        if (maxShield - currentShield < shieldRegenerationRate) {
            currentShield = maxShield;
        }
        else {
            currentShield += shieldRegenerationRate;
        }
    }
}