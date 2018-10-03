using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (BoxCollider2D))]
public class RayCastController : MonoBehaviour {

	public LayerMask collisionMask;
	public BoxCollider2D collider;
	public RaycastOrigins raycastOrigins;
	public const float skinWidth = 0.015f;

	public int horizontalRayCount = 4;
	public int verticalRayCount = 4;

	public float horizontalRaySpacing;
	public float verticalRaySpacing;

	public virtual void Awake() {
		collider = GetComponent<BoxCollider2D>();
	}

	public virtual void Start() {
		CalculateRaySpacing();
	}

	// Creates and updates rays per frame
	public void UpdateRaycastOrigins(){
		Bounds bounds = collider.bounds;
		bounds.Expand(skinWidth * -2);

		raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
		raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
		raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
		raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
	}

	// Calculates the spacing between each ray
	public void CalculateRaySpacing(){
		Bounds bounds = collider.bounds;
		bounds.Expand(skinWidth * -2);

		horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
		verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

		horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
		verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
	}

	public struct RaycastOrigins {
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
	}

}
