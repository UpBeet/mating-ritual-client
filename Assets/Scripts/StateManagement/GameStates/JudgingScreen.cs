using UnityEngine;
using UnityEngine.UI;

// Controls the judging screen.
public class JudgingScreen : IGameState {

	// The space between posed characters on the stage.
	[SerializeField] private float characterSpacing = 3f;

	// Prefab for the button to select a mate.
	[SerializeField] private Button selectMateButtonPrefab;

	// Initialize this game state.
	public override void OnInitializeState () {
		base.OnInitializeState ();
		LoadRigs (new int[] { 0, 1, 2, 4 }, new HandlePositionPairSet[4][]);
	}

	// Load the rigs and their animations on to this screen.
	public void LoadRigs (int[] characterIndices, HandlePositionPairSet[][] animations) {
		Transform container = transform.Find ("Stage");
		container.DestroyAllChildren ();
		for (int i = 0; i < characterIndices.Length; i++) {
			int characterIndex = characterIndices [i];

			// Display the bird and its animation.
			Bird bird = GameController.GetBird (characterIndex);
			PosingController rig = Instantiate (bird.rig);
			rig.transform.SetParent (container);
			rig.transform.localScale = Vector3.one;
			rig.transform.position = new Vector3 (i * characterSpacing, 0, 0);
			rig.PlayAnimation (animations [i]);

			// Display the select mate button.
			Button selectButton = Instantiate (selectMateButtonPrefab);
			selectButton.transform.SetParent (container);
			selectButton.transform.localScale = Vector3.one;
			selectButton.transform.position = new Vector3 (i * characterSpacing, selectMateButtonPrefab.transform.position.y, 0);
			selectButton.onClick.AddListener (() => OnSelectMate (characterIndex));
		}
	}

	// Called when the button to select the mate at a given index is pressed.
	private void OnSelectMate (int index) {
		Debug.LogWarning ("Select mate at character index " + index + ".");
	}
}
