using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// The GameStateManager's primary function is to handle game screens, including calling
/// appropriate methods on them, managing transitions between them, and hosting the game
/// state stack. The GameStateManager also hosts the main CameraController, the
/// GlobalInterfaceController, the GameData, and other persistent information.
/// </summary>
public class GameStateManager : MonoBehaviour {

	/// <summary>
	/// The list of game states, populated in the Unity editor.
	/// </summary>
	public IGameState[] gameStateList;

	/// <summary>
	/// The string key of the game state that is initialized when
	/// the game begins.
	/// </summary>
	public string initialGameStateId;

	/// <summary>
	/// The username of the current player.
	/// </summary>
	public string Username { get; set; }

	/// <summary>
	/// The UUID of the current adventure.
	/// </summary>
	public string CurrentAdventureUUID { get; set; }

	/// <summary>
	/// Dictionary of game state prefabs, collected at load time
	/// from the list of game state prefabs populated in the Unity editor.
	/// </summary>
	private Dictionary<string, IGameState> gameStates;
	
	/// <summary>
	/// The stack of game states stored by their string keys.
	/// </summary>
	private Stack<string> gameStateStack = new Stack<string> ();

	/// <summary>
	/// Reference to the currently active game screen.
	/// </summary>
	private IGameState currentScreen;

	/// <summary>
	/// Initialize the GameStateManager by finding all it's components, hooking to the server,
	/// and assigning the initial GameState.
	/// </summary>
	void Start () {

		// Initialize and load the game state prefabs.
		gameStates = new Dictionary<string, IGameState>();

		// Iterate through the game states in the gameStateList.
		foreach (IGameState state in gameStateList) {

			// If there is a matching id, throw an error.
			if (gameStates.ContainsKey (state.id)) {
				throw new UnityException("Game State ID " + state.id + " is already in use.");
			}

			// Otherwise, cache it in the game state dictionary.
			else {
				gameStates.Add (state.id, state);
			}
		}

		// Load the initial game state.
		SwitchState (initialGameStateId);
	}

	/// <summary>
	/// Transitions to a new game state from the current one. Clears the game
	/// state stack under the new screen.
	/// </summary>
	/// <param id="id">State id.</param>
	public void SwitchState (string id) {

		// Reconfigure stack.
		gameStateStack.Clear ();
		gameStateStack.Push (id);
		
		// Display new screen.
		DisplayNewGameScreen (id);
	}

	/// <summary>
	/// Pushes a new state to the top of the screen stack.
	/// </summary>
	/// <param name="id">Identifier.</param>
	public IGameState PushState (string id) {

		// Push to stack.
		gameStateStack.Push (id);
		
		// Display new screen.
		return DisplayNewGameScreen (id);
	}

	/// <summary>
	/// Pops the current screen off of the screen stack.
	/// </summary>
	public void PopState () {

		// An error is thrown if there is only one state on the stack, do nothing.
		if (gameStateStack.Count == 1) {
			Debug.LogError ("No screen behind the current one.");
			return;
		}

		// Pop off stack.
		gameStateStack.Pop ();
		
		// Display new screen.
		DisplayNewGameScreen (gameStateStack.Peek ());
	}

	/// <summary>
	/// Pops states until the stack reaches a particular state.
	/// </summary>
	/// <param name="name">Name.</param>
	public void PopToState (string name) {

		// Base case: we are already on this screen.
		if (gameStateStack.Peek () == name) {
			return;
		}

		// Keep track of what was popped in the emergency case that this fails.
		Stack<string> popped = new Stack<string> ();

		// Pop states until we find the proper one.
		while (true) {

			// There are no states left to pop.
			if (gameStateStack.Count == 0) {

				// Push all the states back on and exit out.
				while (popped.Count > 0) {
					gameStateStack.Push (popped.Pop ());
				}

				// Mark finished.
				return;
			}
			
			// If we found the screen, display it.
			if (gameStateStack.Peek () == name) {
				
				// Display the new screen.
				DisplayNewGameScreen (gameStateStack.Peek ());
				return;
			}

			// Pop a state.
			popped.Push (gameStateStack.Pop ());
		}
	}

	/// <summary>
	/// Debug outputs the state stack.
	/// </summary>
	public void DebugPrintStateStack () {
		string final = "";
		string[] states = gameStateStack.ToArray ();
		for (int i = 0; i < states.Length; i++) {
			final += states [i];
			if (i != states.Length - 1) {
				final += " / ";
			}
		}

		Debug.LogWarning (final, this);
	}

	/// <summary>
	/// Displays a new game screen, clearing the current screen content.
	/// </summary>
	/// <param name="id">Identifier.</param>
	private IGameState DisplayNewGameScreen (string id) {

		// Destroy previous screen.
		gameObject.DestroyAllChildren();
		
		// Instantiate new screen.
		IGameState state = gameStates[id];
		GameObject go = (GameObject)Instantiate (state.gameObject, transform.position, transform.rotation);
		IGameState stateScreen = go.GetComponent<IGameState> ();
		currentScreen = stateScreen;
		
		// Attach to manager.
		stateScreen.transform.parent = transform;
		stateScreen.Manager = this;
		stateScreen.OnInitializeState ();

		return (IGameState)stateScreen;
	}
}