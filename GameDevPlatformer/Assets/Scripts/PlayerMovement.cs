using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Controller2D))]
public class PlayerMovement : MonoBehaviour {

	public float jumpHeight = 2;
	public float timeToJumpApex = 0.3f;
	public float wallSlideSpeedMax = 0;
	public float wallStickTime = 0.1f;
	public float timeToWallUnstick;
	public float timeToGroundDecelerate;

	public Vector2 wallHop;
	public Vector2 wallLeap;

	float accelerationTimeGrounded = 0.1f;
	float accelerationTimeLanding = 0.3f;
	float moveSpeed = 10;
	float minMoveSpeed = 10;
	float gravity;
	float jumpVelocity;
	float velocityXSmoothing;

	Vector2 velocity;
	Controller2D controller;

	void Start () {
		controller = GetComponent<Controller2D> ();
		// Calculate gravity and jump strength
		gravity = -(2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
	}
		
	void Update () {
		Debug.Log (velocity.x);
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
					velocity.x += 10 * Time.deltaTime;
				} 
				else if (input.x == -1) {
					velocity.x -= 20 * Time.deltaTime;
				}
			} 
			else if (velocity.x < 0) {
				if (input.x == 1) {
					velocity.x += 20 * Time.deltaTime;
				} 
				else if (input.x == -1) {
					velocity.x -= 10 * Time.deltaTime;
				}
			}
		}

		bool wallSliding = false;
		if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0) {
			wallSliding = true;

			// Force vertical velocity limit
			if (velocity.y < -wallSlideSpeedMax) {
				velocity.y = -wallSlideSpeedMax;
			}

			// Give time for player to decide when to jump
			if (timeToWallUnstick > 0) {
				velocityXSmoothing = 0;
				velocity.x = 0;

				if (input.x != wallDirectionX) {
					timeToWallUnstick -= Time.deltaTime;
				} 
				else {
					timeToWallUnstick = wallStickTime;
				}
			} 
			else {
				timeToWallUnstick = wallStickTime;
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

		// Jump input and check if wallsliding or if on ground
		if (Input.GetKeyDown (KeyCode.Space)) {
			if (wallSliding) {
				if (input.x == 0) {
					velocity.x = -wallDirectionX * wallHop.x;
					velocity.y = wallHop.y;
				}
				else {
					velocity.x = -wallDirectionX * wallLeap.x;
					velocity.y = wallLeap.y;
				} 
			}

			if (controller.collisions.below) {
				velocity.y = jumpVelocity;
			}
		}
			
		velocity.y += gravity * Time.deltaTime;
		controller.Move (velocity * Time.deltaTime);
	}
}
