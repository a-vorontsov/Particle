using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerateScript : MonoBehaviour {

	PlayerMovement player;

	void Start() {
		player = GameObject.FindObjectOfType<PlayerMovement> ();
	}
	
	// Detect if player has entered the acceleration zone
	void OnTriggerStay2D(Collider2D col) {
		if (col.gameObject.name == "Player 1") {
			player.moveSpeed += 0.5f;
		} else if (col.gameObject.name == "Player 1") {
			while (player.moveSpeed > 10) {
				player.moveSpeed -= 1;
			}
		}
	}

	// Reset acceleration
	void OnTriggerExit2D(Collider2D col){
		if (col.gameObject.name == "Player 1") {
			player.moveSpeed = 10;
		}
	}
}
