using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHealthBar : MonoBehaviour {

    public GameObject healthBar;
    public GameObject shieldBar;
    public float healthCanvasSize;
    public float shieldCanvasSize;
    public float maxCanvasSize = 200;

    public void updateHealthShield(float currentHealth, float maxHealth, float currentShield, float maxShield) {
        healthCanvasSize = currentHealth / maxHealth * maxCanvasSize;
        healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(healthCanvasSize, healthBar.GetComponent<RectTransform>().sizeDelta.y);

        shieldCanvasSize = currentShield / maxShield * maxCanvasSize;
        shieldBar.GetComponent<RectTransform>().sizeDelta = new Vector2(shieldCanvasSize, shieldBar.GetComponent<RectTransform>().sizeDelta.y);
    }
}
