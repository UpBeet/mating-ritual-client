using UnityEngine;
using System.Collections;

// Controls the judging screen.
public class JudgingScreen : IGameState {

	// The space between posed characters on the stage.
	[SerializeField] private float characterSpacing = 3f;

	// Initialize this game state.
	public override void OnInitializeState () {
		base.OnInitializeState ();
		LoadRigs (new int[] { 0, 1, 2, 4 }, new HandlePositionPairSet[4][]);
	}

	// Load the rigs and their animations on to this screen.
	public void LoadRigs (int[] characterIndices, HandlePositionPairSet[][] animations) {
		Debug.LogWarning (characterIndices.Length);
		for (int i = 0; i < characterIndices.Length; i++) {
			Bird bird = GameController.GetBird (characterIndices [i]);
			PosingController rig = Instantiate (bird.rig);
			rig.transform.SetParent (transform);
			rig.transform.localScale = Vector3.one;
			rig.transform.position = new Vector3 (i * characterSpacing, 0, 0);
			rig.PlayAnimation (animations [i]);
		}
	}
}
