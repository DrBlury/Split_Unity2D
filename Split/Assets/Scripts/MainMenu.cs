using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void PlayStoryGame() {
        SceneManager.LoadScene("Intro");
    }

    public void PlayEndlessGame() {
        SceneManager.LoadScene("Endless");
    }

    public void Quit() {
        Application.Quit();
    }
}
