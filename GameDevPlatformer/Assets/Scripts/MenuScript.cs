using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

	// Start game on press
	public void StartGame (string levelName) {
		SceneManager.LoadScene (levelName);
	}

	// Quit game on press (doesn't function in unity editor)
	public void ClickExit () {
		Application.Quit ();
	}
}
