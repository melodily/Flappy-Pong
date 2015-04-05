using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour
{

	public float amountToMove = 6;
	public float speed = 0.8f;
	void OnTriggerEnter2D (Collider2D other)
	{
//		Collider2D other = col.collider;

		if (other.GetComponent<Player> () != null) {
//			Camera.main.transform.Translate (amountToMove);
			StartCoroutine (Move ());
		}
	}
	IEnumerator Move ()
	{
		Debug.Log ("Entered");
		float amountMoved = 0;
		while (amountMoved<amountToMove) {
			Debug.Log ("moving");
			Camera.main.transform.Translate (Vector3.right * speed);
			amountMoved += speed;
			yield return null;
		}
	}
}
