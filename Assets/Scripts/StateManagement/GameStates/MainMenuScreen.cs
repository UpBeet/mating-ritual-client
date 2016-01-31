using UnityEngine;

// Controls the main menu screen.
public class MainMenuScreen : IGameState {

	// Called when the host game button is clicked.
	public void OnClickHostGameButton () {
		Debug.LogWarning ("Host game not hooked to server. Moving to test this functionality client-side.");
		BeginHostingGame ("ABCD");
	}

	// Called when the server responds to a request to host the game.
	private void BeginHostingGame (string roomkey) {
		DataCache.RoomKey = roomkey;
		DataCache.IsHost = true;
		PushState ("Lobby");
	}
}
