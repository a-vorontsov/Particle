using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerateScript : MonoBehaviour {

	public GameObject player;

	public bool accelerating;

	void Start () {
		accelerating = false;
	}
	
	// Detect if player has entered the acceleration zone
	void OnTriggerStay2D (Collider2D col){
		if (col.gameObject.name == "Player 1"){
			accelerating = true;
		}
	}

	// Reset acceleration
	void OnTriggerExit2D (Collider2D col){
		if (col.gameObject.name == "Player 1"){
			accelerating = false;
		}
	}
}
