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
	}
}
