using UnityEngine;
using UnityEngine.EventSystems;

// Controls a single handle.
public class HandleController : MonoBehaviour {

	// Every handle needs an ID that the server refers to it by.
	public long id;

	// The hideable components of the handles.
	private HideableInterfaceElement[] hideables;

	// Initialize this component.
	void Start () {
		hideables = GetComponentsInChildren<HideableInterfaceElement> ();
		SetVisible (true);
	}

	#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER

	// Called when the mouse drags this component.
	void OnMouseDrag () {
		Vector3 worldPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		transform.position = new Vector2 (worldPoint.x, worldPoint.y);
	}

	#endif

	// Sets whether or not this handle is visible.
	public void SetVisible (bool visible) {

		if (hideables == null) {
			hideables = GetComponentsInChildren<HideableInterfaceElement> ();
		}

		// Show/hide handles.
		for (int i = 0; i < hideables.Length; i++) {
			hideables [i].SetShown (visible);
		}

		// Enable/disable collider.
		GetComponent<Collider2D> ().enabled = visible;
	}

	// Fires when this handle begins dragging.
	public void OnBeginDrag () {
	}

	// Fires while this handle is being dragged.
	public void OnDrag () {
		transform.position = Input.GetTouch (0).position;
	}
}
