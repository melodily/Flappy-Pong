using UnityEngine;
using System.Collections;

public class DestroyObjects : MonoBehaviour
{


//		public Transform ball;
//		Vector3 ballDistanceFromCamera;
//		void Start ()
//		{
//		
//		}
//	void OnCollisionEnter2D (Collision2D col)
//	{
//		Collider2D other = col.collider;
//		if (other.tag == "Ball") {
////			Debug.DrawRay (other.transform.position, -col.contacts [0].normal, Color.red, 3f);
////			other.rigidbody2D.velocity += (-col.contacts [0].normal * 6f);
//			LevelGenerator.Instance.Restart (other);
//		}
//	}
	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Ball") {
			LevelGenerator.Instance.Restart (other);
		}
		if (other.GetComponent<MovingObstacle> () != null) {
			LevelGenerator.Instance.movingObstacles.Remove (other.gameObject);
		}
		if (other.tag == "Obstacle") {
			LevelGenerator.Instance.obstacles.Remove (other.gameObject);
		}
		PowerUp powerup = other.GetComponent<PowerUp> ();
		if (powerup != null) {
			if (powerup.type == PowerUp.Types.Invulnerable) {
				if (!powerup.taken) {
					LevelGenerator.Instance.invulPool.Remove (other.gameObject);
				}
			} else {
				LevelGenerator.Instance.healthPool.Remove (other.gameObject);
			}
		}

		if (other.GetComponent<PointTrigger> () != null) {
			LevelGenerator.Instance.pointTriggers.Remove (other.gameObject);
		}

	}
}
