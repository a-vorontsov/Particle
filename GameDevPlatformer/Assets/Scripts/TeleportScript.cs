using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportScript : MonoBehaviour {

	public GameObject player;

	public Vector2 destination;

	public float countdownTimer;

	public bool teleporting;

	void Start () {
		teleporting = false;
		countdownTimer = 0.1f;
	}

	void Update () {
		if (!teleporting) {
			countdownTimer = 0.1f;
		}
	}

	// Detect if player has entered TP zone and change position
	void OnTriggerStay2D (Collider2D col){
		if (col.gameObject.name == "Player 1") {
			teleporting = true;
			// Begin timer
			if (countdownTimer > 0) {
				countdownTimer -= Time.deltaTime;
			}
			//Teleport after timer drops below 0
			else if (countdownTimer <= 0) {
				player.transform.position = new Vector2 (destination.x, destination.y);
			}
		}
	}

	// Reset timer on player exit
	void OnTriggerExit2D (Collider2D col) {
		if (col.gameObject.name == "Player 1") {
			if (countdownTimer > 0) {
				countdownTimer -= Time.deltaTime;
			}
			//Teleport after timer drops below 0
			else if (countdownTimer <= 0) {
				teleporting = false;
			}
		}
	}
}
