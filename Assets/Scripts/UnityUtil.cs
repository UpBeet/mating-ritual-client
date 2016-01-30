using UnityEngine;

/// <summary>
/// Global library of static tools used on Unity engine data throughout the project.
/// </summary>
public static class UnityUtil {

	/// <summary>
	/// Destroys all the children attached to this game object.
	/// </summary>
	/// <param name="gameObject">The specified parent game object.</param>
	public static void DestroyAllChildren (this GameObject gameObject) {
		DestroyAllChildren (gameObject.transform);
	}

	/// <summary>
	/// Destroys all the children attached to this transform.
	/// </summary>
	/// <param name="transform">The specified parent transform.</param>
	public static void DestroyAllChildren (this Transform transform) {
		for (int i = 0; i < transform.childCount; i++) {
			Object.Destroy (transform.GetChild (i).gameObject);
		}
	}

	/// <summary>
	/// Properly gets the Collider2D under the relevant input.
	/// </summary>
	/// <returns>The Collider2D under input.</returns>
	public static Collider2D GetCollider2DUnderInput () {

		// Get the input position.
		Vector2 input = GetInputPosition ();

		// Get the collider under the input and return it.
		return Physics2D.OverlapPoint (input);
	}

	/// <summary>
	/// Properly gets the Collider2D under the relevant input.
	/// </summary>
	/// <returns>The Collider2D under input.</returns>
	/// <param name="layer">The string name of the LayerMask to restrict.</param>
	public static Collider2D GetCollider2DUnderInput (string layer) {

		// Get the input position.
		Vector2 input = GetInputPosition ();

		// Get the collider under the input and return it.
		return Physics2D.OverlapPoint (input, 1 << LayerMask.NameToLayer (layer));
	}

	/// <summary>
	/// Gets the platform-relevant input position.
	/// </summary>
	/// <returns>The input position.</returns>
	public static Vector2 GetInputPosition () {

		// Default to mouse position.
		Vector2 input = Input.mousePosition;

		// Use input to determine world position in 2D space.
		Vector3 wp = Camera.main.ScreenToWorldPoint (input);
		Vector2 inputPosition = new Vector2 (wp.x, wp.y);

		// Return the calculated position.
		return inputPosition;
	}

	/// <summary>
	/// Breadth first search of a Transform for a child of a given name.
	/// </summary>
	/// <returns>Child Transform with given name, null if not found.</returns>
	/// <param name="parent">Parent to search.</param>
	/// <param name="name">Name of child.</param>
	public static Transform BreadthFirstSearchChildren (this Transform parent, string name) {

		// If the child is found in the immediate children, return it.
		Transform result = parent.Find (name);
		if (result != null)
			return result;

		// Otherwise, recursively search immediate children.
		foreach (Transform child in parent) {
			result = child.BreadthFirstSearchChildren (name);
			if (result != null)
				return result;
		}

		// If it isn't found, return null.
		return null;
	}

	/// <summary>
	/// Depth first search of a Transform for a child of a given name.
	/// </summary>
	/// <returns>Child Transform with given name, null if not found.</returns>
	/// <param name="parent">Parent to search.</param>
	/// <param name="name">Name of child.</param>
	public static Transform DepthFirstSearchChildren (this Transform parent, string name) {

		// Search immediate children.
		foreach (Transform child in parent) {
			// If name is found, return Transform.
			if (child.name == name)
				return child;

			// Otherwise search deeper.
			Transform result = child.DepthFirstSearchChildren (name);
			if (result != null)
				return result;
		}

		// If it isn't found, return null.
		return null;
	}
}
