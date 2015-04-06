using UnityEngine;
using System.Collections;

public class DestroyItself : MonoBehaviour
{

	// Update is called once per frame
	void Update ()
	{
	
		if (!LevelGenerator.Instance.isGameOver && transform.position.y < -5f) {
			LevelGenerator.Instance.Restart (collider2D, true);
		}
	
	}
}
