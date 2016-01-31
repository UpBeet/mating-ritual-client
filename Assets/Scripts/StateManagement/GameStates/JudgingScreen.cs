using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

// Controls the judging screen.
public class JudgingScreen : IGameState {

	// The space between posed characters on the stage.
	[SerializeField] private float characterSpacing = 10f;

	// Prefab for the button to select a mate.
	[SerializeField] private Button selectMateButtonPrefab;

	// Animation that gets instantiated when the winner is shown.
	[SerializeField] private GameObject winnerAnimation;

	// Camera end bounds on right side.
	private float cameraEndBound = 0;

	// Dictionary of rigs by character index.
	private Dictionary<int, PosingController> rigs;

	// Initialize this game state.
	public override void OnInitializeState () {
		base.OnInitializeState ();

		// Properly sort background.
		GuiCanvas.worldCamera = Camera.main;
		GuiCanvas.sortingLayerName = "Floor";

		// Test loading rigs.
		LoadRigs (new int[] { 0, 1, 2, 4 }, new HandlePositionPairSet[4][]);

		// Ensure camera drag component.
		CameraDrag drag = Camera.main.GetComponent<CameraDrag> ();
		if (drag == null) {
			drag = Camera.main.gameObject.AddComponent<CameraDrag> ();
		}
		drag.vertical = false;
	}

	// Update this game state.
	public override void OnUpdateState () {
		base.OnUpdateState ();

		// Clamp camera to bounds.
		float cameraX = Camera.main.transform.position.x;
		cameraX = Mathf.Clamp (cameraX, 0, cameraEndBound);
		Camera.main.transform.position = new Vector3 (cameraX, Camera.main.transform.position.y, Camera.main.transform.position.z);
	}

	// Clean up this game state before exiting.
	public override void OnExitState () {
		base.OnExitState ();
		Destroy (Camera.main.GetComponent<CameraDrag> ());
	}

	// Load the rigs and their animations on to this screen.
	public void LoadRigs (int[] characterIndices, HandlePositionPairSet[][] animations) {
		Transform container = transform.Find ("Stage");
		container.DestroyAllChildren ();
		rigs = new Dictionary<int, PosingController> ();
		for (int i = 0; i < characterIndices.Length; i++) {
			int characterIndex = characterIndices [i];

			// Display the bird and its animation.
			Bird bird = GameController.GetBird (characterIndex);
			PosingController rig = Instantiate (bird.rig);
			rig.transform.SetParent (container);
			rig.transform.localScale = Vector3.one;
			rig.transform.position = new Vector3 (i * characterSpacing, 0, 0);
			rig.PlayAnimation (animations [i]);
			rigs.Add (characterIndex, rig);

			// Display the select mate button.
			Button selectButton = Instantiate (selectMateButtonPrefab);
			selectButton.transform.SetParent (container);
			selectButton.transform.localScale = Vector3.one;
			selectButton.transform.position = new Vector3 (i * characterSpacing, selectMateButtonPrefab.transform.position.y, 0);
			selectButton.onClick.AddListener (() => OnSelectMate (characterIndex));
		}
		cameraEndBound = characterSpacing * (characterIndices.Length - 1);
	}

	// Called when the button to select the mate at a given index is pressed.
	private void OnSelectMate (int index) {
		Debug.LogWarning ("Select mate at character index " + index + ".");
		ShowWinner (index);
	}

	// Show the winner at the specified index.
	private void ShowWinner (int index) {
		Bird bird = GameController.GetBird (index);
		Vector2 winnerPos = rigs [index].transform.position;

		// Drag to winner.
		CameraDrag drag = Camera.main.GetComponent<CameraDrag> ();
		drag.horizontal = false;
		drag.Focus (winnerPos);

		// Show animation.
		GameObject anim = Instantiate (winnerAnimation);
		anim.transform.localScale = Vector3.one;
		anim.transform.position = winnerPos;
		anim.transform.SetParent (transform);

		// Play bird sound.
		Camera.main.GetComponent<AudioSource> ().PlayOneShot (bird.sound);
	}
}
