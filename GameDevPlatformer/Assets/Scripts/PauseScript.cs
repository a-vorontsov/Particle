using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour {

	public bool isPaused;

	public GameObject pauseMenuCanvas;

	// Resume game on key hit
	public void Resume() {
		isPaused = false;
	}

	// Quit to main menu on press
	public void QuitGame() {
		Application.LoadLevel ("Main Menu");
	}
	
	// Update is called once per frame
	void Update () {
		if (isPaused) {
			pauseMenuCanvas.SetActive (true);
			Time.timeScale = 0;
		} 
		else {
			pauseMenuCanvas.SetActive (false);
			Time.timeScale = 1;
		}

		if (Input.GetKeyDown (KeyCode.Escape)) {
			isPaused = !isPaused;
		}
		else if (isPaused && Input.GetKeyDown (KeyCode.R)) {
				isPaused = false;
		}
	}
}
