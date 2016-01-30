using UnityEngine;
using UnityEngine.UI;

// Forces an input field to convert all text to uppercase.
[ExecuteInEditMode, RequireComponent (typeof (InputField))]
public class UppercaseInputField : MonoBehaviour {

	// Update this component.
	void Update () {
		InputField input = GetComponent<InputField> ();
		input.text = input.text.ToUpper ();
	}
}
