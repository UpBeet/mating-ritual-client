using UnityEngine;

/// <summary>
/// Spawns a prefab to its position and then destroys itself.
/// </summary>
public class PrefabSpawner : MonoBehaviour {

	public GameObject prefab;

	void Awake () {
		GameObject instantiated = Instantiate (prefab) as GameObject;
		instantiated.transform.SetParent (transform.parent);
		instantiated.transform.localPosition = transform.localPosition;
		instantiated.transform.localScale = transform.localScale;
		instantiated.transform.localRotation = transform.localRotation;
		Destroy (gameObject);
	}
}
