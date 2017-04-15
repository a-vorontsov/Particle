using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecelerateScript : MonoBehaviour {

	public GameObject player;

	public bool decelerating;

	void Start () {
		decelerating = false;
	}
	
	// Detect if player has entered the deceleration zone
	void OnTriggerStay2D (Collider2D col){
		if (col.gameObject.name == "Player 1"){
			decelerating = true;
		}
	}

	// Reset deceleration
	void OnTriggerExit2D (Collider2D col){
		if (col.gameObject.name == "Player 1"){
			decelerating = false;
		}
	}
}
