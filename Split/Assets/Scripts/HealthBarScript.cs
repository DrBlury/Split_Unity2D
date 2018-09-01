using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarScript : MonoBehaviour {
    public GameObject target;
    public GameObject owner;
    public GameObject healthBar;
    public GameObject shieldBar;
    public Vector3 Offset;
    public float healthCanvasSize;
    public float shieldCanvasSize;
    public float maxCanvasSize = 200;
    bool isInitialized;
    Vector3 targetPos;

    public void Initialize(GameObject iowner, GameObject itarget, Vector3 iOffset) {
        isInitialized = true;
        owner = iowner;
        target = itarget;
        Offset = iOffset;
    }

    public void updateHealthShield(float currentHealth, float maxHealth, float currentShield, float maxShield) {
        healthCanvasSize = currentHealth / maxHealth * maxCanvasSize;
        healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(healthCanvasSize, healthBar.GetComponent<RectTransform>().sizeDelta.y);

        shieldCanvasSize = currentShield / maxShield * maxCanvasSize;
        shieldBar.GetComponent<RectTransform>().sizeDelta = new Vector2(shieldCanvasSize, shieldBar.GetComponent<RectTransform>().sizeDelta.y);
    }

    private void Update() {
        if (owner != null) {
            transform.position = owner.transform.position + Offset;
        }
        else {
            if (isInitialized) {
                Destroy(gameObject);
            }

        }

    }

}
