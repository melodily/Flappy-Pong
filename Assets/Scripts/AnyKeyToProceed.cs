using UnityEngine;
using System.Collections;

public class AnyKeyToProceed : MonoBehaviour
{


	void Update ()
	{
		if (Input.anyKeyDown) {
			Application.LoadLevel (Application.loadedLevel + 1);
		}
	}
}
