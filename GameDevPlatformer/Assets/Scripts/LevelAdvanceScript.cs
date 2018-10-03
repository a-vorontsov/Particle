using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelAdvanceScript : MonoBehaviour {

	public bool isWon;

	public GameObject nextLevelMenuCanvas;
	public GameObject player;

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.name == "Player 1") {
			isWon = true;
		}
	}

	// Resume game on key hit
	public void Resume() {
		isWon = false;
	}

	// Quit to main menu on press
	public void QuitGame() {
		SceneManager.LoadScene("Main Menu");
	}

	// Load next level
	public void NextLevel(string nextLevelName) {
		SceneManager.LoadScene(nextLevelName);
	}

	void Update() {
		// Stop player movement
		if (isWon) {
			nextLevelMenuCanvas.SetActive(true);
		} else {
			nextLevelMenuCanvas.SetActive(false);
		}

		// Reset and unpause
		if (isWon && Input.GetKeyDown(KeyCode.R)) {
			isWon = false;
		}

		// Quit the game on key press
		if (isWon && Input.GetKeyDown(KeyCode.Escape)) {
			SceneManager.LoadScene("Main Menu");
		}
	}
}
