using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlipScript : MonoBehaviour {

	public GameObject player;

	public bool slipping;

	void Start () {
		slipping = false;
	}
	
	// Detect if player has entered the slipping zone
	void OnTriggerStay2D (Collider2D col){
		if (col.gameObject.name == "Player 1"){
			slipping = true;
		}
	}

	// Reset slipping
	void OnTriggerExit2D (Collider2D col){
		if (col.gameObject.name == "Player 1"){
			slipping = false;
		}
	}
}
