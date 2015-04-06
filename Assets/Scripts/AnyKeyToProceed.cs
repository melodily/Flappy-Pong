using UnityEngine;
using System.Collections;

public class AnyKeyToProceed : MonoBehaviour
{

	public bool isInstructions = false;
	void Update ()
	{
		if (isInstructions) {
			if (Input.GetKeyDown (KeyCode.Alpha2) || Input.GetKeyDown (KeyCode.Keypad2)) {
				Application.LoadLevel (Application.loadedLevel + 2);
			} else if (Input.GetKeyDown (KeyCode.Alpha3) || Input.GetKeyDown (KeyCode.Keypad3)) {
				Application.LoadLevel (Application.loadedLevel + 3);
			} else {
				if (Input.anyKeyDown) {
					Application.LoadLevel (Application.loadedLevel + 1);
				}
			}
		} else if (Input.anyKeyDown) {
			Application.LoadLevel (Application.loadedLevel + 1);
		}
	
	}
}
