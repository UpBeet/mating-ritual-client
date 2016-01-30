using UnityEngine;
using System.Collections;

public abstract class IGameStateChild : MonoBehaviour {

	/// <summary>
	/// Gets or sets the parent game state.
	/// </summary>
	/// <value>The master.</value>
	protected IGameState Master { get; private set; }

	/// <summary>
	/// Called when this child game state gets initialized by its parent
	/// game state.
	/// </summary>
	public virtual void OnInitializeState (IGameState master) {
		this.Master = master;
	}

	/// <summary>
	/// Called when this child game state gets updated by its parent
	/// game state.
	/// </summary>
	public virtual void OnUpdateState () {
	}

	/// <summary>
	/// Called before this child's parent state itself exits.
	/// </summary>
	public virtual void OnExitState () {
	}
}
