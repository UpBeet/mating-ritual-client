using UnityEngine;

// Manages the screen where users pose their characters
public class PosingScreen : IGameState {

	public int numFrames = 3;

	// Reference to the rig in the scene.
	private PosingController rig;

	private int currentFrame = 0;

	// The stored keyframes for the pending animation.
	private HandlePositionPairSet[] keyFrames;

	// Initialize this game state.
	public override void OnInitializeState () {
		base.OnInitializeState ();
		rig = GetComponentInChildren<PosingController> ();
		keyFrames = new HandlePositionPairSet [numFrames];
	}

	// Save the pose to the current key frame and increment.
	public void SavePose () {
		keyFrames [currentFrame] = rig.CollectHandlePositions ();
		currentFrame++;

		if (currentFrame >= numFrames) {
			rig.PlayAnimation (keyFrames);
		}
	}
}
