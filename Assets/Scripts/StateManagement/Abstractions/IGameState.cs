using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for game states managed by a GameStateManager. Game states
/// should control all the GameObjects and data relevant to a single screen
/// in the game.
/// </summary>
public class IGameState : MonoBehaviour {

	/// <summary>
	/// The name used by the GameStateManager to identify this game state.
	/// </summary>
	public string id;

	/// <summary>
	/// The GameStateManager this game state is managed by.
	/// </summary>
	public GameStateManager Manager {get; set;}
	
	/// <summary>
	/// Dictionary of hideable interface elements present on this state.
	/// </summary>
	private Dictionary<string, HideableInterfaceElement> hideableElements;

	/// <summary>
	/// Dictionary of child game states that need to be intiialized, updated,
	/// and communicated with by this master game state.
	/// </summary>
	private Dictionary<string, IGameStateChild> stateChildren;

	/// <summary>
	/// The GUI canvas used to draw the user interface for this screen.
	/// </summary>
	public Canvas GuiCanvas { get; protected set; }
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	public void Update () {
		OnUpdateState ();
	}

	/// <summary>
	/// Raises the initialize state event.
	/// </summary>
	public virtual void OnInitializeState () {
		hideableElements = new Dictionary<string, HideableInterfaceElement> ();
		GuiCanvas = transform.Find ("GUI Canvas").GetComponent<Canvas> ();

		// Find and initialize children game states.
		stateChildren = new Dictionary<string, IGameStateChild> ();
		Component[] children = GetComponentsInChildren<IGameStateChild> (true);
		foreach (IGameStateChild child in children) {
			child.OnInitializeState (this);
			stateChildren.Add (child.name, child);
		}
	}

	/// <summary>
	/// Raises the update state event.
	/// </summary>
	public virtual void OnUpdateState () {

		// Update children.
		foreach (IGameStateChild child in stateChildren.Values) {
			child.OnUpdateState ();
		}
	}

	/// <summary>
	/// Raises the exit state event.
	/// </summary>
	public virtual void OnExitState () {
		// Exit children.
		if (stateChildren != null) {
			foreach (IGameStateChild child in stateChildren.Values) {
				child.OnExitState ();
			}
		}
	}

	/// <summary>
	/// Transitions to a new game state from the current one via the
	/// attached manager. Wipes all states currently on the game state
	/// stack.
	/// </summary>
	/// <param id="id">State id.</param>
	public void SwitchState (string id) {
		Manager.SwitchState (id);
	}

	/// <summary>
	/// Pushes a new state on top of this one on the game state stack.
	/// </summary>
	/// <param name="id">Identifier.</param>
	public void PushState (string id) {
		Manager.PushState (id);
	}

	/// <summary>
	/// Pushes a new state on top of this one, and returns a reference to it.
	/// </summary>
	/// <returns>Reference to the game state pushed.</returns>
	/// <param name="id">Identifier.</param>
	public IGameState GetPushState (string id) {
		return Manager.PushState (id);
	}

	/// <summary>
	/// Pops this state off of the screen stack.
	/// </summary>
	public void PopState () {
		Manager.PopState ();
	}

	/// <summary>
	/// Shows the hideable element with the given name.
	/// </summary>
	/// <param name="name">Name.</param>
	protected void ShowGUIElement (string name) {
		hideableElements[name].Show ();
	}

	/// <summary>
	/// Hides the hideable element with the given name.
	/// </summary>
	/// <param name="name">Name.</param>
	protected void HideGUIElement (string name) {
		hideableElements[name].Hide ();
	}
	
	/// <summary>
	/// Gets the hideable element with the given name.
	/// </summary>
	/// <param name="name">Name.</param>
	protected HideableInterfaceElement GetGUIElement (string name) {
		return hideableElements[name];
	}
	
	/// <summary>
	/// Adds and initializes a hideable element.
	/// Assumed child of guiCanvas, hidden by default.
	/// </summary>
	/// <param name="name">Name of GameObject.</param>
	protected void AddHideableElement (string name) {
		AddHideableElement (name, GuiCanvas.transform, true);
	}
	
	/// <summary>
	/// Adds and initializes a hideable element.
	/// Hidden by default.
	/// </summary>
	/// <param name="name">Name of GameObject.</param>
	/// <param name="parent">Parent Transform.</param>
	protected void AddHideableElement (string name, Transform parent) {
		AddHideableElement (name, parent, true);
	}

	/// <summary>
	/// Adds and initializes a hideable element.
	/// Hidden by default.
	/// </summary>
	/// <param name="name">Name of GameObject.</param>
	/// <param name="startsHidden">If set to <c>true</c>, starts hidden.</param>
	protected void AddHideableElement (string name, bool startsHidden) {
		AddHideableElement (name, GuiCanvas.transform, startsHidden);
	}

	/// <summary>
	/// Adds and initializes a hideable element.
	/// </summary>
	/// <param name="name">Name of GameObject.</param>
	/// <param name="parent">Parent Transform.</param>
	/// <param name="startsHidden">If set to <c>true</c>, starts hidden.</param>
	protected void AddHideableElement (string name, Transform parent, bool startsHidden) {
		HideableInterfaceElement element = parent.FindChild (name).GetComponent<HideableInterfaceElement> ();
		hideableElements.Add (name, element);
		if (startsHidden) element.HideImmediate ();
		element.CanvasGroup.alpha = 0;
	}

	/// <summary>
	/// Searches for and adds and initializes a hideable element.
	/// </summary>
	/// <param name="name">Name of GameObject.</param>
	/// <param name="startsHidden">If set to <c>true</c> starts hidden.</param>
	/// <param name="deepSearch">If set to <c>true</c> depth first search.</param>
	protected void SearchHideableElement (string name, bool startsHidden = true, bool deepSearch = false) {
		HideableInterfaceElement element;

		if (deepSearch) {
			element = GuiCanvas.transform.DepthFirstSearchChildren (name).GetComponent<HideableInterfaceElement> ();
		}
		else {
			element = GuiCanvas.transform.BreadthFirstSearchChildren (name).GetComponent<HideableInterfaceElement> ();
		}

		hideableElements.Add (name, element);
		if (startsHidden) element.HideImmediate ();
	}

	/// <summary>
	/// Gets a game state child by it's game object name.
	/// </summary>
	/// <param name="name">Name of game object.</param>
	protected IGameStateChild GetChild (string name) {
		return stateChildren [name];
	}
}
