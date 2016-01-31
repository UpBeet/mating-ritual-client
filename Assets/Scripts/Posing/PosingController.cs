using UnityEngine;
using System.Collections.Generic;

// Relationship between a handle and a position.
public class HandlePositionPair {
	public long handleID;
	public Vector2 position;
}

// Set of handle-position-pairs.
public class HandlePositionPairSet {
	public List<HandlePositionPair> handlePositionPairs = new List<HandlePositionPair> ();

	// Get handle-position pair by the handle ID.
	public HandlePositionPair GetPairByHandleID (long handleID) {
		for (int i = 0; i < handlePositionPairs.Count; i++) {
			HandlePositionPair pair = handlePositionPairs [i];
			if (pair.handleID == handleID) {
				return pair;
			}
		}
		return null;
	}
}

// Manages a single posed rig.
public class PosingController : MonoBehaviour {

	// Time between each keyframe.
	[SerializeField] private float timeBetweenKeyframes = 2f;

	// Animation time tracker.
	private float t = 0;

	// Current animation frame.
	private int currentFrame = 0;

	// References to all the handles in this rig.
	private HandleController[] handles;

	// Set of handle-position pairs to play across keyframes.
	private HandlePositionPairSet[] playingKeyframes;

	// If set to true, this posing controller is just playing an animation and can't be controlled.
	private bool playing = false;

	// Initialize this component.
	void Start () {
		handles = GetComponentsInChildren<HandleController> ();
	}

	// Update this component.
	void Update () {
		if (playing) {

			// Update time.
			t += Time.deltaTime;
			if (t >= timeBetweenKeyframes) {
				t -= timeBetweenKeyframes;
				currentFrame = (currentFrame + 1) % playingKeyframes.Length;
			}

			// Get the next frame.
			int nextFrame = (currentFrame + 1) % playingKeyframes.Length;

			// Interpolate the positions of each of the handles.
			HandlePositionPairSet currentKey = playingKeyframes [currentFrame];
			HandlePositionPairSet nextKey = playingKeyframes [nextFrame];
			for (int i = 0; i < handles.Length; i++) {
				HandleController handle = handles [i];
				HandlePositionPair current = currentKey.GetPairByHandleID (handle.id);
				HandlePositionPair next = nextKey.GetPairByHandleID (handle.id);
				handle.transform.position = new Vector2 (
					Mathf.Lerp (current.position.x, next.position.x, t / timeBetweenKeyframes),
					Mathf.Lerp (current.position.y, next.position.y, t / timeBetweenKeyframes)
				);
			}
		}
	}

	// Sets whether or not all the handles are visible.
	public void SetAllHandlesVisible (bool visible) {
		if (handles == null) {
			handles = GetComponentsInChildren<HandleController> ();
		}
		for (int i = 0; i < handles.Length; i++) {
			handles [i].SetVisible (visible);
		}
	}

	// Collect the set of handle position pairs.
	public HandlePositionPairSet CollectHandlePositions () {
		HandlePositionPairSet pairs = new HandlePositionPairSet ();
		for (int i = 0; i < handles.Length; i++) {
			HandleController handle = handles [i];
			pairs.handlePositionPairs.Add (
				new HandlePositionPair {
					handleID = handle.id,
					position = handle.transform.position,
				}
			);
		}
		return pairs;
	}

	// Play an animation given a bunch of handle-position pairs and frames.
	public void PlayAnimation (HandlePositionPairSet[] pairs) {

		// Can't see handles while playing.
		SetAllHandlesVisible (false);

		// Check if there are actually pairs to work with.
		if (pairs == null || pairs.Length <= 0) {
			return;
		}

		// Copy pairs and start playing.
		playingKeyframes = pairs;
		playing = true;
	}

	// Get a handle controller via its ID.
	public HandleController GetHandleByID (long handleID) {
		for (int i = 0; i < handles.Length; i++) {
			HandleController handle = handles [i];
			if (handle.id == handleID) {
				return handle;
			}
		}
		return null;
	}
}
