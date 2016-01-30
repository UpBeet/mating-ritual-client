using UnityEngine;

// Controls the main menu screen.
public class JoinGameScreen : IGameState {

	// The currently entered room code.
	public string RoomCode { get; set; }

	// Called when the join game button is clicked.
	public void OnClickJoinGameButton () {
		Debug.LogWarning ("Attempting to join room " + RoomCode + ".");
		BeginJoinGame (RoomCode);
	}

	// Called when the server responds to a request to join a game.
	private void BeginJoinGame (string roomkey) {
		DataCache.RoomKey = roomkey;
		DataCache.IsHost = false;
		PushState ("Lobby");
	}
}
