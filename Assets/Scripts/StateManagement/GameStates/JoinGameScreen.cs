using UnityEngine;
using UnityEngine.UI;

// Controls the join game screen.
public class JoinGameScreen : IGameState {

	// The currently entered room code.
	public string RoomCode { get; set; }

	// Called when the join game button is clicked.
	public void OnClickJoinGameButton () {
        BirbClient client = GameObject.Find("BirbClient").GetComponent<BirbClient>();
        //Debug.LogWarning ("Attempting to join room " + RoomCode + ".");

        // Check for input.
        if (string.IsNullOrEmpty (RoomCode)) {
			GetComponentInChildren<InputField> ().Select ();
			return;
		}

        DataCache.RoomKey = RoomCode;
        client.SendBirbMessage(BirbClient.BirbMessageCode.JOIN_ROOM, DataCache.RoomKey, BeginJoinGame);
    }

    private void BeginJoinGame(params object[] parameters)
    {
        if(int.Parse(parameters[0].ToString()) > -1)
        {
            BeginJoinGame(int.Parse(parameters[0].ToString()));
        }
        else
        {
            Debug.LogError("Invalid room key.");
            // TODO: Display message indicating that the room doesn't exist
        }
    }

	// Called when the server responds to a request to join a game.
	private void BeginJoinGame (int userId) {
        DataCache.PlayerIndex = userId;
		DataCache.IsHost = false;
		PushState ("Lobby");
	}
}
