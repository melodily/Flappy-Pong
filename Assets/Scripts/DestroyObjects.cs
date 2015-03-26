using UnityEngine;
using System.Collections;

public class DestroyObjects : MonoBehaviour
{

		public bool shouldDestroyBall = true;

//		public Transform ball;
//		Vector3 ballDistanceFromCamera;
//		void Start ()
//		{
//		
//		}
		void OnCollisionStay2D (Collision2D col)
		{
				Collider2D other = col.collider;
				if (other.tag == "Ball") {
						LevelGenerator.Instance.Restart ();
				}
		}
		void OnTriggerEnter2D (Collider2D other)
		{
				if (other.tag == "Obstacle") {
						Debug.Log ("destroying obstacle");
						LevelGenerator.Instance.obstacles.Remove (other.gameObject);
				}
				if (other.GetComponent<PointTrigger> () != null) {
						LevelGenerator.Instance.pointTriggers.Remove (other.gameObject);
				}
				if (shouldDestroyBall && other.tag == "Ball") {
						LevelGenerator.Instance.Restart ();
				}
		}
}
