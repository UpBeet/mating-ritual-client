using UnityEngine;

// Controls the main menu screen.
public class JoinGameScreen : IGameState {

	// The currently entered room code.
	public string RoomCode { get; set; }

	// Called when the join game button is clicked.
	public void OnClickJoinGameButton () {
		Debug.LogWarning ("Join game not hooked to server. Moving to test this functionality client-side.");
	}
}
