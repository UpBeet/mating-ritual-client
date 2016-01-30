using UnityEngine;
using UnityEngine.UI;

// Manages the lobby screen.
public class LobbyScreen : IGameState {

	// Initialize this game state.
	public override void OnInitializeState () {
		base.OnInitializeState ();
		transform.Find ("GUI Canvas/Content/Room Code").GetComponent<Text> ().text = DataCache.RoomKey;
	}
}
