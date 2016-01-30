using UnityEngine;

// Controls the main menu screen.
public class MainMenuScreen : IGameState {

	// Called when the host game button is clicked.
	public void OnClickHostGameButton () {
		Debug.LogWarning ("Host game not hooked to server. Moving to test this functionality client-side.");
		BeginHostingGame ("ABCD");
	}

	// Called when the join game button is clicked.
	public void OnClickJoinGameButton () {
		Debug.LogWarning ("Join game not implemented yet.");
	}

	// Called when the rules button is clicked.
	public void OnClickRulesButton () {
		Debug.LogWarning ("Rules not implemented yet.");
	}

	// Called when the server responds to a request to host the game.
	private void BeginHostingGame (string roomkey) {
		DataCache.RoomKey = roomkey;
		DataCache.IsHost = true;
		PushState ("Lobby");
	}
}
