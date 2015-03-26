using UnityEngine;
using System.Collections;

public class DestroyItself : MonoBehaviour
{

		// Update is called once per frame
		void Update ()
		{
	
				if (transform.position.y < -5f) {
						LevelGenerator.Instance.Restart ();
				}
	
		}
}
