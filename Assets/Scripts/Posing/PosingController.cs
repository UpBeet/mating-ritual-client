using UnityEngine;

// Relationship between a handle and a position.
public class HandlePositionPair {
	long handleID;
	Vector2 position;
}

// Manages a single posed rig.
public class PosingController : MonoBehaviour {

	// References to all the handles in this rig.
	HandleController[] handles;

	// Initialize this component.
	void Start () {
		handles = GetComponentsInChildren<HandleController> ();
	}

	// Sets whether or not all the handles are visible.
	public void SetAllHandlesVisible (bool visible) {
		for (int i = 0; i < handles.Length; i++) {
			handles [i].SetVisible (visible);
		}
	}
}
