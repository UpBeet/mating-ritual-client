using UnityEngine;
using UnityEngine.UI;

// Controls the join game screen.
public class JoinGameScreen : IGameState {

	// The currently entered room code.
	public string RoomCode { get; set; }

	// Called when the join game button is clicked.
	public void OnClickJoinGameButton () {
		Debug.LogWarning ("Attempting to join room " + RoomCode + ".");

		// Check for input.
		if (string.IsNullOrEmpty (RoomCode)) {
			GetComponentInChildren<InputField> ().Select ();
			return;
		}

		// Test join game.
		BeginJoinGame (RoomCode.ToUpper ());
	}

	// Called when the server responds to a request to join a game.
	private void BeginJoinGame (string roomkey) {
		DataCache.RoomKey = roomkey;
		DataCache.IsHost = false;
		PushState ("Lobby");
	}
}
