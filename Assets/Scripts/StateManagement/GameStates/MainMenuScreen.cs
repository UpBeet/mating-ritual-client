using UnityEngine;

// Controls the main menu screen.
public class MainMenuScreen : IGameState {

	// Called when the host game button is clicked.
	public void OnClickHostGameButton () {
        //Debug.LogWarning ("Host game not hooked to server. Moving to test this functionality client-side.");
        BirbClient client = GameObject.Find("BirbClient").GetComponent<BirbClient>();
        client.SendBirbMessage(BirbClient.BirbMessageCode.CREATE_ROOM, "", BeginHostingGame);
	}

	// Called when the server responds to a request to host the game.
	private void BeginHostingGame (params object[] parameters) {
        Debug.Log("Beginning hosting game callback");
        BeginHostingGame(parameters[0].ToString());
	}

    private void BeginHostingGame(string roomKey)
    {
        BirbClient client = GameObject.Find("BirbClient").GetComponent<BirbClient>();
        client.SendBirbMessage(BirbClient.BirbMessageCode.JOIN_ROOM, DataCache.RoomKey, ChangeScreens);
    }

    private void ChangeScreens(params object[] parameters)
    {
        Debug.Log("Running change screens callback");
        DataCache.IsHost = true;
        PushState("Lobby");
    }
}
