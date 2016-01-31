﻿using UnityEngine;
using UnityEngine.UI;

// Manages the lobby screen.
public class LobbyScreen : IGameState {

	// Participating bird icon prefab;
	[SerializeField] private RectTransform participatingBirdIconPrefab;

	// Initialize this game state.
	public override void OnInitializeState () {
		base.OnInitializeState ();
		transform.Find ("GUI Canvas/Content/Room Code").GetComponent<Text> ().text = DataCache.RoomKey;
		ShowHostControls (DataCache.IsHost);
		SetParticipatingBirds (new []{
			0, 3, 4, 7
		});
	}

	// Set the birds currently participating in this game.
	public void SetParticipatingBirds (int[] birdIndices) {
		RectTransform container = transform.Find ("GUI Canvas/Content/Joined Characters Panel").GetComponent<RectTransform> ();
		container.DestroyAllChildren ();
		for (int i = 0; i < birdIndices.Length; i++) {
			int birdIndex = birdIndices [i];
			if (birdIndex != DataCache.PlayerIndex) {
				Bird bird = GameController.GetBird (birdIndex);
				RectTransform birdIcon = Instantiate (participatingBirdIconPrefab);
				birdIcon.SetParent (container);
				birdIcon.GetComponent<Image> ().sprite = bird.headSprite;
				birdIcon.localScale = Vector3.one;
			}
		}
	}

	// Called when the start button is pressed.
	public void OnPressStartButton () {
		Debug.LogWarning ("Start game not implemented server-side. Initalizing client-side testing.");
		GameStarted (3);
	}

	// Sets whether or not the host controls are shown.
	private void ShowHostControls (bool isHost) {
		transform.Find ("GUI Canvas/Content/Waiting For Host Text").gameObject.SetActive (!isHost);
		transform.Find ("GUI Canvas/Content/Start Game Panel").gameObject.SetActive (isHost);
	}

	// Call this when the game is started from the server by the host.
	private void GameStarted (int judgeIndex) {
		DataCache.JudgeIndex = judgeIndex;
		PopState ();
		PushState (DataCache.PlayerIsJudge () ? "Prompt" : "Scores");
	}
}
