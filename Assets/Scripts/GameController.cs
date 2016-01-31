using UnityEngine;
using System;
using System.Collections;

// A bird.
[Serializable]
public class Bird {
	public string name;
	public Sprite headSprite;
	public PosingController rig;
}

// Manages the GameController component.
public class GameController : MonoBehaviour {

	// Array of birds.
	[SerializeField] private Bird[] birds;

	// Singleton instance of the GameController component.
	private static GameController singleton;

	// Initialize this component.
	void Awake () {
		singleton = this;
	}

	// Get a bird by its index.
	public static Bird GetBird (int index) {
		return singleton.birds [index];
	}
}
