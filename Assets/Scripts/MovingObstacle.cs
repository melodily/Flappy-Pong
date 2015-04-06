using UnityEngine;
using System.Collections;

public class MovingObstacle : MonoBehaviour
{

	public float speed = 0.1f;
	public float moveAmt = 3f;
	public int dir = 1;
	float amountMoved;
	
	void OnEnable ()
	{
		amountMoved = 0;
		dir = 1;
	}
	
	void FixedUpdate ()
	{
		if (amountMoved < moveAmt) {
			transform.Translate (Vector3.up * speed * dir);
			amountMoved += speed;
		} else {
			amountMoved = 0;
			dir *= -1;
		}

	}
}
