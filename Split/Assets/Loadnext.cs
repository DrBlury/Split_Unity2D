using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loadnext : MonoBehaviour {

    public float timeToNextLevel;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        timeToNextLevel -= Time.deltaTime;

        if (timeToNextLevel <= 0) {
            SceneManager.LoadScene("Level1");
        }
	}

    public void skip() {
        SceneManager.LoadScene("Level1");
    }
}
