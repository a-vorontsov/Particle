using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour {

	// Start game on press
	public void StartGame() {
		Application.LoadLevel ("platformer 1");
	}

	// Quit game on press (doesn't function in unity editor)
	public void ClickExit() {
		Application.Quit ();
	}
}
