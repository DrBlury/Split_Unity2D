using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour {
    [Header("General Settings")]
    public bool canRegenerate;

    [Header("Health Settings")]
    public GameObject myHealthBar;
    public float maxHealth = 200;

    [Header("Shield Settings")]
    public float maxShield = 200;
    public float shieldRegenerationDelay = 3;
    public float shieldRegenerationRate = 20;

    [Header("Debug Info")]
    [SerializeField] float currentHealth;
    [SerializeField] float currentShield;
    [SerializeField] bool isRegenShield;
    [SerializeField] public static float timestamp = 0.0f;

    void Start() {
        currentHealth = maxHealth;
        currentShield = maxShield;
    }

    private void Update() {
        if (canRegenerate) {
            if (currentShield != maxShield && !isRegenShield) {
                StartCoroutine(RegainShieldOverTime());
            }
        }

        if (currentHealth <= 0) {
            GetComponent<Animator>().SetBool("isDead", true);
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            Destroy(gameObject, 0.5f);
            SceneManager.LoadScene("Goal");
            //TODO Show Game over screen
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
        myHealthBar.GetComponent<UIHealthBar>().updateHealthShield(currentHealth, maxHealth, currentShield, maxShield);
    }

    public void UsePowerUp(string powerUpType) {
        if (powerUpType == "heal") {
            if (currentHealth + 50 <= maxHealth) {
                currentHealth = currentHealth + 50;
            } else {
                currentHealth = maxHealth;
            }
            myHealthBar.GetComponent<UIHealthBar>().updateHealthShield(currentHealth, maxHealth, currentShield, maxShield);
        } else if (powerUpType == "speed") {
            StartCoroutine(ShootingSpeedUp());
        }
    }

    public void LifeSteal(int heal) {
        if (currentHealth + heal <= maxHealth) {
            currentHealth = currentHealth + heal;
        } else {
            currentHealth = maxHealth;
        }
        myHealthBar.GetComponent<UIHealthBar>().updateHealthShield(currentHealth, maxHealth, currentShield, maxShield);
    }
    private IEnumerator ShootingSpeedUp() {
        GetComponent<ShootingScript>().delayInSeconds = GetComponent<ShootingScript>().delayInSeconds * 0.5f;
        yield return new WaitForSeconds(5);
        GetComponent<ShootingScript>().delayInSeconds = GetComponent<ShootingScript>().delayInSeconds * 2f;
    }

    private IEnumerator RegainShieldOverTime() {
        isRegenShield = true;
        while (currentShield < maxShield && Time.time > (timestamp + shieldRegenerationDelay)) {
            Shieldregen();
            yield return new WaitForSeconds(1);
        }
        isRegenShield = false;
    }

    public void Shieldregen() {
        if (maxShield - currentShield < shieldRegenerationRate) {
            currentShield = maxShield;
        }
        else {
            currentShield += shieldRegenerationRate;
        }
        myHealthBar.GetComponent<UIHealthBar>().updateHealthShield(currentHealth, maxHealth, currentShield, maxShield);
    }
}
