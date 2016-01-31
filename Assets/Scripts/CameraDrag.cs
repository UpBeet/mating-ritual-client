﻿using UnityEngine;

/// <summary>
/// Attach to a Camera component to enable touch and drag or click and drag. Moves along the XY plane currently
/// (assumed orthographic or vertical view).
/// </summary>
[RequireComponent (typeof(Camera))]
public class CameraDrag : MonoBehaviour {

	/// <summary>
	/// The drag speed coefficient.
	/// </summary>
	public float dragSpeed = 0.005f;

	// Horizontal drag enabled.
	public bool horizontal = true;

	// Vertical drag enabled.
	public bool vertical = true;

	/// <summary>
	/// The drag position last frame.
	/// </summary>
	private Vector2 prevDragPosition = Vector2.zero;

	/// <summary>
	/// The current drag velocity.
	/// </summary>
	private Vector2 velocity = Vector2.zero;

	/// <summary>
	/// The position that the camera is focusing on.
	/// </summary>
	private Vector2 focusPosition = Vector2.zero;

	/// <summary>
	/// If set to true, still moving to focus.
	/// </summary>
	private bool movingToFocus = false;

	/// <summary>
	/// The focus transform.
	/// </summary>
	private Transform focus = null;

	/// <summary>
	/// If the camera is close to its focus than the focus threshold, it will stop moving on its own.
	/// </summary>
	[SerializeField] private float focusThreshold = 0.1f;

	/// <summary>
	/// The speed at which focusing occurs.
	/// </summary>
	[SerializeField] private float focusRate = 0.1f;

	/// <summary>
	/// Reference to the camera component.
	/// </summary>
	private Camera cam;

	/// <summary>
	/// Initialize this component.
	/// </summary>
	void Start () {
		cam = GetComponent<Camera> ();
		focusPosition = transform.position;
	}

	/// <summary>
	/// Update this component.
	/// </summary>
	void Update () {

		#if (UNITY_EDITOR || UNITY_STANDALONE)

		if (Input.GetMouseButtonDown(0)) {
			prevDragPosition = Input.mousePosition;
		}

		if (Input.GetMouseButton (0)) {
			Vector2 currentMousePosition = Input.mousePosition;
			Vector2 deltaMousePosition = currentMousePosition - prevDragPosition;
			velocity = deltaMousePosition * dragSpeed * cam.orthographicSize;
			velocity = new Vector2 (horizontal ? velocity.x : 0, vertical ? velocity.y : 0);
			prevDragPosition = currentMousePosition;
		}
		else {
			velocity *= 0.9f;
		}

		#endif

		// Check if we still have to focus a specific point.
		if (movingToFocus) {

			// Check if we need to update the focus point because we are following a transform.
			if (focus != null) {
				focusPosition = focus.position;
			}

			// Check if we need to get closer to the focus.
			if (Vector2.Distance (transform.position, focusPosition) > focusThreshold) {
				transform.position = Vector2.Lerp (transform.position, focusPosition, focusRate);
			}
			else {

				// Resolve based on whether or not we are following a specific transform.
				if (focus != null) {
					transform.localPosition = Vector2.zero;
				}
				else {
					transform.position = focusPosition;
				}

				// End motion.
				velocity = Vector2.zero;
				Debug.Log ("stopped");
				movingToFocus = false;
			}

			// Make sure the camera is backed up while game takes control of camera.
			transform.position = new Vector3 (transform.position.x, transform.position.y, -100);
		}
		else {

			// Update using control velocity.
			transform.position = new Vector3 (transform.position.x - velocity.x,
				transform.position.y - velocity.y, -100);
		}

		// Lock local rotation.
		transform.rotation = Quaternion.identity;
	}

	/// <summary>
	/// Focus on the specified point.
	/// </summary>
	/// <param name="focusPosition">Focus position.</param>
	public void Focus (Vector2 focusPosition) {
		Debug.Log (focusPosition, this);
		this.focusPosition = focusPosition;
		movingToFocus = true;
	}

	/// <summary>
	/// Follow the specified transform.
	/// </summary>
	/// <param name="focus">Focused transform.</param>
	public void Follow (Transform focus) {
		Debug.Log (focus, this);
		this.focus = focus;
		transform.SetParent (focus);
		if (focus != null) {
			Focus (focus.position);
		}
		else {
			movingToFocus = false;
		}
	}
}
