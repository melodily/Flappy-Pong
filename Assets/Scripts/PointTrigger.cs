using UnityEngine;
using System.Collections;

public class PointTrigger : MonoBehaviour
{

		void OnTriggerExit2D (Collider2D other)
		{
				if (other.tag == "Ball") {
						LevelGenerator.Instance.IncreaseScore ();
				}
		}
}
