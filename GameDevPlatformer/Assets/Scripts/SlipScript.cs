using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlipScript : MonoBehaviour {

	PlayerMovement player;

	void Start () {
		player = GameObject.FindObjectOfType<PlayerMovement> ();
	}
	
	// Detect if player has entered the slipping zone
	void OnTriggerStay2D (Collider2D col){
		if (col.gameObject.name == "Player 1"){
			player.wallSliding = false;
		}
	}

	// Reset slipping
	void OnTriggerExit2D (Collider2D col){
		if (col.gameObject.name == "Player 1"){
			player.wallSliding = true;
		}
	}
}
