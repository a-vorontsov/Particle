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

	public Vector2 wallHop;
	public Vector2 wallLeap;

	float accelerationTimeGrounded = 0.1f;
	float moveSpeed = 5;
	float minMoveSpeed = 5;
	float gravity;
	float jumpVelocity;
	float velocityXSmoothing;

	Vector2 velocity;
	Controller2D controller;

	IEnumerator Wait() {
		yield return new WaitForSeconds (2f);
	}

	void Start () {
		controller = GetComponent<Controller2D> ();
		// calculate gravity and jump strength
		gravity = -(2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
	}
		
	void Update () {
		print (velocity.x);
		// Movement input
		Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
		int wallDirectionX = (controller.collisions.left) ? -1 : 1;
		float targetVelocityXGround = input.x * moveSpeed;
		// Reduce horizontal acceleration on ground after waiting
		if (controller.collisions.below) {
			
			StartCoroutine (Wait ());
			velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityXGround, ref velocityXSmoothing, accelerationTimeGrounded);
		} 
		// Accelerate while airbourne
		else {
			if (velocity.x > 0) {
				velocity.x += 10 * Time.deltaTime;
			} 
			else if (velocity.x < 0) {
				velocity.x -= 10 * Time.deltaTime;
			}
		}

		bool wallSliding = false;
		if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0) {
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
				

		// Jump and double jump input and check
		if (Input.GetKeyDown (KeyCode.Space)) {
			velocity.x += 10 * Time.deltaTime;
			if (wallSliding) {
				if ( wallDirectionX == 0) {
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
