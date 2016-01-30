using UnityEngine;
using UnityEngine.UI;

// Manages the lobby screen.
public class LobbyScreen : IGameState {

	// Initialize this game state.
	public override void OnInitializeState () {
		base.OnInitializeState ();
		transform.Find ("GUI Canvas/Content/Room Code").GetComponent<Text> ().text = DataCache.RoomKey;
		ShowHostControls (DataCache.IsHost);
	}

	// Sets whether or not the host controls are shown.
	private void ShowHostControls (bool isHost) {
		transform.Find ("GUI Canvas/Content/Waiting For Host Text").gameObject.SetActive (!isHost);
		transform.Find ("GUI Canvas/Content/Start Game Panel").gameObject.SetActive (isHost);
	}
}
