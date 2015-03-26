using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

	// Use this for initialization
	public float speed = 3f;
	public float forceAddRate = 0.05f;
	public float horizontalForce = 1f;
	public GameObject forceBar; 
	float accumulatedForce = 0;
	public float maxForce = 13;
	public AudioSource bounce, charge;
	float maxTimeAllowedOnPaddle = 0.05f;
	float timer;
		
	[HideInInspector]
	public bool
		canBounce = true;


	// Update is called once per frame
	void Update ()
	{
		if (LevelGenerator.isGameStarted) {
			Vector3 vel = Vector3.zero;
			if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) {
				vel.x = -speed;
			}
			if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) {
				vel.x = speed;
			}
			if ((Input.GetKey (KeyCode.Space))) {
				//charge.Play ();
				accumulatedForce = Mathf.Min (accumulatedForce + forceAddRate, maxForce);
			}
			rigidbody2D.velocity = vel;
			Vector3 scale = forceBar.transform.localScale;
			scale.x = accumulatedForce / maxForce;
			forceBar.transform.localScale = scale;
		}
	}

	void OnCollisionEnter2D (Collision2D col)
	{
		Collider2D other = col.collider;
		if (other.tag == "Ball") {
			if (!canBounce) {
				LevelGenerator.Instance.Restart ();
			} else {
				Vector3 vel = Vector3.zero;
				vel.x = horizontalForce;
				vel.y = accumulatedForce;
//						collider2D.sharedMaterial.bounciness = accumulatedForce;
				other.rigidbody2D.velocity = vel;
				bounce.Play ();
				accumulatedForce = 0;
			}
			canBounce = false;
			timer = 0;
		}
	}
		
	void OnCollisionStay2D (Collision2D col)
	{
		Collider2D other = col.collider;
		if (other.tag == "Ball") {
			timer += Time.deltaTime;
			if (timer > maxTimeAllowedOnPaddle) {
				LevelGenerator.Instance.Restart ();
			}
		}
	}
		
}
