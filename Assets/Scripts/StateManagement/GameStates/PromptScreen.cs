using UnityEngine;
using UnityEngine.UI;

// Controls the judge's initial prompt screen.
public class PromptScreen : IGameState {

	// The currently entered response to the prompt.
	public string Response { get; set; }

	// The base prompt.
	[SerializeField] private string prompt;

	// Initialize this game state.
	public override void OnInitializeState () {
		base.OnInitializeState ();
		transform.Find ("GUI Canvas/Prompt").GetComponent<Text> ().text = prompt;
	}

	// Called when the submit button is clicked.
	public void OnClickSubmitButton () {
		Debug.LogWarning ("Sending response: " + prompt + " " + Response + ".");
        BirbClient client = GameObject.Find("BirbClient").GetComponent<BirbClient>();
        client.SendBirbMessage(BirbClient.BirbMessageCode.SEND_PROMPT, prompt + " " + transform.Find("GUI Canvas/Response Input Input/Text").GetComponent<Text>().text, DataCache.RoomKey, BeginWaitingForPoses);
    }

    private void BeginWaitingForPoses(params object[] parameters)
    {
        BeginWaitingForPoses();
    }

    private void BeginWaitingForPoses()
    {
		transform.Find("GUI Canvas/Submit Button").GetComponent<Button>().interactable = false;
		transform.Find ("GUI Canvas/Response Input").GetComponent<InputField> ().interactable = false;
        transform.Find("GUI Canvas/Prompt").GetComponent<Text>().text = "Waiting for contestants to pose.";
    }
}
