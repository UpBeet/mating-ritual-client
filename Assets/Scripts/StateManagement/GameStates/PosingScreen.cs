using UnityEngine;

// Manages the screen where users pose their characters
public class PosingScreen : IGameState {

	// References to all the handles in the scene.
	private HandleController[] handles;

	// Initialize this game state.
	public override void OnInitializeState () {
		base.OnInitializeState ();
		handles = GetComponentsInChildren<HandleController> ();
	}
	
	// Sets whether or not all the handles are visible.
	public void SetAllHandlesVisible (bool visible) {
		for (int i = 0; i < handles.Length; i++) {
			handles [i].SetVisible (visible);
		}
	}
}
