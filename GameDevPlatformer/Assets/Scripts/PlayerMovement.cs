using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Controller2D))]
public class PlayerMovement : MonoBehaviour {

	public float jumpHeight = 2;
	public float timeToJumpApex = 0.3f;
	public float wallSlideSpeedMax = 0;
	public float wallStickTime = 0.05f;
	public float timeToWallUnstick;

	public Vector2 wallClimb;
	public Vector2 wallHop;
	public Vector2 wallLeap;

	float accelerationTimeAirborne = 0.2f;
	float accelerationTimeGrounded = 0.1f;
	float moveSpeed = 6;
	float gravity;
	float jumpVelocity;
	float velocityXSmoothing;

	int doubleJump = 0;

	bool wallSliding = false;
	bool canWallSlide = true;

	Vector2 velocity;
	Controller2D controller;

	// Use this for initialization
	void Start () {
		controller = GetComponent<Controller2D> ();
		// calculate gravity and jump strength
		gravity = -(2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
	}

	public IEnumerator stopWallSlide() {
		yield return new WaitForSeconds (0.5f);
		wallSliding = false;
		canWallSlide = false;
		yield return new WaitUntil(() => controller.collisions.below || controller.collisions.left || controller.collisions.right);
		canWallSlide = true;
	}

	// Update is called once per frame
	void Update () {
		// Movement input
		Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
		int wallDirectionX = (controller.collisions.left) ? -1 : 1;

		float targetVelocityX = input.x * moveSpeed;
		// Reduce horizontal acceleration in mid air
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

		if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0 && canWallSlide == true) {
			wallSliding = true;
			if (velocity.y < -wallSlideSpeedMax) {
				velocity.y = -wallSlideSpeedMax;
			}

			if (timeToWallUnstick > 0) {
				velocityXSmoothing = 0;
				velocity.x = 0;
				if (input.x != wallDirectionX && input.x != 0) {
					timeToWallUnstick -= Time.deltaTime;
				} 
				else {
					timeToWallUnstick = wallStickTime;
				}
			} 
			else {
				timeToWallUnstick = wallStickTime;
			}
			StartCoroutine (stopWallSlide ());
		}

		// Stop gravity build up while grounded
		if (controller.collisions.above || controller.collisions.below) {
			velocity.y = 0;
		}

		// Jump and double jump input and check
		if (Input.GetKeyDown (KeyCode.Space)) {
			if (wallSliding) {
				if (wallDirectionX == -input.x) {
					velocity.x = -wallDirectionX * wallLeap.x;
					velocity.y = wallLeap.y;
					doubleJump = 2;
					canWallSlide = true;
				} 
				else {
					velocity.x = -wallDirectionX * wallHop.x;
					velocity.y = wallHop.y;
					doubleJump = 2;
					canWallSlide = true;
				} 
			}
			//Recalculate gravity and jump strength mid air
			if (doubleJump != 2) {
				gravity = -(2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
				jumpVelocity = Mathf.Abs (gravity) * timeToJumpApex;
				velocity.y = jumpVelocity;
				doubleJump += 1;
				jumpHeight = 1.5f;
			}
		}

		// Double jump reset on landing
		if (controller.collisions.below && velocity.y == 0) {
			doubleJump = 0;
			jumpHeight = 2;
		}
			
		velocity.y += gravity * Time.deltaTime;
		controller.Move (velocity * Time.deltaTime);
	}
}
