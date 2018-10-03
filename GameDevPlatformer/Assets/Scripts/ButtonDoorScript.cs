using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDoorScript : MonoBehaviour {
	public GameObject door;

	public Vector2 destination;
	public Vector2 originalLocation;

	SpriteRenderer renderer;

	public bool activated;

	void Start () {
		renderer = GetComponent<SpriteRenderer>();
		activated = false;
		originalLocation = new Vector2(originalLocation.x, originalLocation.y);
		this.renderer.color = new Color(255, 255, 0, 255);
	}
	
	// Change button colour and door position on player collision
	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.name == "Player 1") {
			activated = true;
			door.transform.position = new Vector2(destination.x, destination.y);
			this.renderer.color = new Color(255, 0, 0, 255);
		}
	}

	void Update() {
		// Reset buttons/doors on level reset
		if (Input.GetKeyDown (KeyCode.R)){
			door.transform.position = originalLocation;
			activated = false;
			this.renderer.color = new Color(255, 255, 0, 255);
		}
	}
}
