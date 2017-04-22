using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Controller2D))]
public class PlayerMovement : MonoBehaviour {

	public float jumpHeight = 2;
	public float timeToGroundDecelerate;
	public float timeToJumpApex = 0.3f;
	public float timeToWallUnstick;
	public float wallSlideSpeedMax = 0;
	public float wallStickTime = 0;
	public float minYPosition;
	public float moveSpeed;

	public Vector2 spawnPoint;
	public Vector2 velocity;
	public Vector2 wallClimb;
	public Vector2 wallHop;
	public Vector2 wallLeap;

	float accelerationTimeGrounded = 0.1f;
	float accelerationTimeLanding = 0.3f;
	float minMoveSpeed = 10;
	float gravity;
	float jumpVelocity;
	float velocityXSmoothing;

	AccelerateScript accelerate;
	DecelerateScript decelerate;
	SlipScript slipping;
	Controller2D controller;
	GameObject player;
	SpriteRenderer renderer;
	TeleportScript teleport;
	TrailRenderer trail;
	PauseScript pause;
	LevelAdvanceScript levelAdvance;

	void Start () {
		pause = GameObject.FindObjectOfType<PauseScript> ();
		levelAdvance = GameObject.FindObjectOfType<LevelAdvanceScript> ();
		accelerate = GameObject.FindObjectOfType<AccelerateScript> ();
		decelerate = GameObject.FindObjectOfType<DecelerateScript> ();
		slipping = GameObject.FindObjectOfType<SlipScript> ();
		controller = GetComponent<Controller2D> ();
		renderer = GetComponent<SpriteRenderer> ();
		teleport = GameObject.FindObjectOfType<TeleportScript> ();
		trail = GetComponent<TrailRenderer> ();

		// Calculate gravity and jump strength
		gravity = -(2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		spawnPoint = new Vector2 (spawnPoint.x, spawnPoint.y);
		moveSpeed = 10;
	}
		
	void Update () {
		// Movement input
		Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
		int wallDirectionX = (controller.collisions.left) ? -1 : 1;
		float targetVelocityXGround = input.x * moveSpeed;
		// Reduce horizontal acceleration on ground after waiting
		if (controller.collisions.below) {
			if (input.x == 1) {
				velocity.x += 100 * Mathf.Pow(Time.deltaTime, 2);
			} 
			else if (input.x == -1) {
				velocity.x -= (100 * Mathf.Pow(Time.deltaTime, 2));
			}
			if (timeToGroundDecelerate < 0.25f) {
				if (Mathf.Sign (input.x) != Mathf.Sign (velocity.x)) {
					velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityXGround, ref velocityXSmoothing, accelerationTimeLanding);
				} 
				else {
					timeToGroundDecelerate += Time.deltaTime;
				}
			}
			else if (timeToGroundDecelerate >= 0.25f) {
				velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityXGround, ref velocityXSmoothing, accelerationTimeGrounded);
			}
		}
		// Accelerate while airborne and slow down when input is opposite to movement
		else if (!controller.collisions.below) {
			timeToGroundDecelerate = 0;
			if (velocity.x > 0) {
				if (input.x == 1) {
					velocity.x += 20 * Time.deltaTime;
				} 
				else if (input.x == -1) {
					velocity.x -= 40 * Time.deltaTime;
				}
			} 
			else if (velocity.x < 0) {
				if (input.x == 1) {
					velocity.x += 40 * Time.deltaTime;
				} 
				else if (input.x == -1) {
					velocity.x -= 20 * Time.deltaTime;
				}
			}
		}

		bool wallSliding = false;
		if (!slipping.slipping && (controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0) {
			wallSliding = true;

			// Force vertical velocity limit
			if (velocity.y < -wallSlideSpeedMax) {
				velocity.y = wallSlideSpeedMax;
			}
		}

		// Stop gravity build up while grounded and decelerate fast
		if (controller.collisions.above || controller.collisions.below) {
			velocity.y = 0;
			if (velocity.x < -minMoveSpeed) {
				velocity.x += 5 * Time.deltaTime;
			} 
			else if (velocity.x > minMoveSpeed) {
				velocity.x -= 5 * Time.deltaTime;
			}
		}

		// stop velocity build up while colliding with a wall
		if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below) {
			velocity.x = 0;
		}

		// Jump input and check if wallsliding or if on ground
		if (Input.GetKeyDown (KeyCode.Space) || Input.GetKeyDown (KeyCode.UpArrow)) {
			if (wallSliding) {
				if (input.x == 0) {
					velocity.x = -wallDirectionX * wallHop.x;
					velocity.y = wallHop.y;
				} 
				else if (input.x != wallDirectionX) {
					velocity.x = -wallDirectionX * wallLeap.x;
					velocity.y = wallLeap.y;
				} 
				else if (input.x == wallDirectionX) {
					velocity.x = -wallDirectionX * wallClimb.x;
					velocity.y = wallClimb.y;
				}
			}

			if (controller.collisions.below) {
				velocity.y = jumpVelocity;
			}
		}

		// Make Player move
		velocity.y += gravity * Time.deltaTime;
		controller.Move (velocity * Time.deltaTime);

		// Respawn player after falling 50 units below spawn or after pressing R
		if ((this.transform.position.y < (minYPosition)) || Input.GetKeyDown (KeyCode.R)) {
			this.transform.position = spawnPoint;
			// Reset velocity to prevent abuse
			velocity.x = 0;
			velocity.y = 0;
			// Disable trail
			teleport.teleporting = false;;
			trail.time = 0;
			this.renderer.enabled = true;
		}

		// Trail distance change on velocity change
		if (velocity.x > 10 || velocity.x < -10  || velocity.y > 10 || velocity.y < -10) {
			trail.enabled = true;
			if (trail.time < 0.25f) {
				trail.time += 0.01f;
			} 
			else {
				trail.time = 0.25f;
			}
		} 

		// Reduce trail length when slow
		else {
			if (trail.time > 0) {
				trail.time -= 0.01f;
			}
			else {
				trail.enabled = false;
			}
		}

		// Make player (in)visible while teleporting
		if (teleport.teleporting) {
			this.renderer.enabled = false;
			trail.time = 0;
			velocity.x = 0;
		} 
		else if (!teleport.teleporting) {
			this.renderer.enabled = true;
			trail.enabled = true;
		}

		// Accelerate player when in zone and moving
		if (accelerate.accelerating && (input.x != 0)) {
			moveSpeed = moveSpeed + 1;
		}
		else if (accelerate.accelerating && (input.x == 0)) {
			while (moveSpeed > 10) {
				moveSpeed = moveSpeed - 1;
			}
		}

		// Decelerate player when in zone and moving
		if (decelerate.decelerating && (input.x != 0) && moveSpeed > 5) {
			moveSpeed = moveSpeed - 1;
		}
		else if (decelerate.decelerating && (input.x == 0)) {
			while (moveSpeed < 10) {
				moveSpeed = moveSpeed + 1;
			}
		}
		else if (!decelerate.decelerating && !accelerate.accelerating) {
			moveSpeed = 10;
		}

		if (!pause.isPaused && !levelAdvance.isWon) {
			Time.timeScale = 1;
		}
		else {
			Time.timeScale = 0;
		}
	}
}
