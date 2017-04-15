using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour {

	public bool isPaused;

	public GameObject pauseMenuCanvas;

	// Quit to main menu on press
	public void QuitGame() {
		SceneManager.LoadScene ("Main Menu");
	}
	

	void Update () {
		// Set canvas
		if (isPaused) {
			pauseMenuCanvas.SetActive (true);
		} 
		else {
			pauseMenuCanvas.SetActive (false);
		}

		// Toggle if (un)paused or reset and unpause
		if (Input.GetKeyDown (KeyCode.Escape)) {
			isPaused = !isPaused;
		}
		else if (isPaused && Input.GetKeyDown (KeyCode.R)) {
				isPaused = false;
		}
	}
}
