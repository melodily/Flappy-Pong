using UnityEngine;
using System.Collections;

public class PointTrigger : MonoBehaviour
{
	bool taken = false;
	void OnEnable ()
	{
		taken = false;
	}

	void OnTriggerExit2D (Collider2D other)
	{
		if (!taken && other.tag == "Ball") {
			LevelGenerator.Instance.IncreaseScore ();
			taken = true;
		}
	}
}
