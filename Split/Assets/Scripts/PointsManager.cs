using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointsManager : MonoBehaviour {
    public GameObject scoretext;
    [SerializeField] private int scorePoints = 0;

    public void AddPoints(int points) {
        scorePoints += points;
        scoretext.GetComponent<TextMeshProUGUI>().text = "Score: " + scorePoints.ToString();
    }
}
