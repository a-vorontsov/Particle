using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportScript : MonoBehaviour {

	public GameObject player;

	public Vector2 destination;

	public float countdownTimer;

	PlayerMovement velocity;

	void Start () {
		velocity = GameObject.FindObjectOfType<PlayerMovement>();
		countdownTimer = 0.05f;
	}

	// Detect if player has entered TP zone and change position
	void OnTriggerStay2D(Collider2D col) {
		if (col.gameObject.name == "Player 1") {
			// Begin timer
			if (countdownTimer > 0) {
				countdownTimer -= Time.deltaTime;
			} else if (countdownTimer <= 0) {
				player.transform.position = new Vector2(destination.x, destination.y);
				velocity.velocity.x = 0;
				velocity.velocity.y = 0;
				velocity.trail.time = 0;
			}
		}
	}

	// Reset timer on player exit
	void OnTriggerExit2D(Collider2D col) {
		if (col.gameObject.name == "Player 1") {
			countdownTimer = 0.05f;
		}
	}
}
