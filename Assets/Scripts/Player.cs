using UnityEngine;
using System.Collections;

public class Player : Singleton<Player>
{

	// Use this for initialization
	public float speed = 10f;
	public float forceAddRate = 0.6f;
	public float horizontalForce = 3f;
	public GameObject forceBar; 
	protected float accumulatedForce = 0;
	public float maxForce = 13;
	public AudioSource bounce, charge;
	protected float maxTimeAllowedOnPaddle = 0.05f;
	protected float timer;
	public Vector3 gravity;
	void Start ()
	{
		Physics2D.gravity = gravity;
	}
	void OnDestroy ()
	{
		Physics2D.gravity = new Vector3 (0, -9.81f, 0);
	}
		
	[HideInInspector]
	public bool
		canBounce = true;


	// Update is called once per frame
	protected virtual void Update ()
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
//			Vector3 pos = transform.position;
//			pos.x = LevelGenerator.Instance.ball.position.x;
//			transform.position = pos;
			rigidbody2D.velocity = vel;
			Vector3 scale = forceBar.transform.localScale;
			scale.x = accumulatedForce / maxForce;
			forceBar.transform.localScale = scale;
		}
	}

	protected virtual void OnCollisionEnter2D (Collision2D col)
	{
		Collider2D other = col.collider;
		if (other.tag == "Ball") {
//			if (!canBounce) {
//				LevelGenerator.Instance.Restart ();
//			} else {
			Vector3 vel = Vector3.zero;
			vel.x = horizontalForce;
			vel.y = accumulatedForce;
//						collider2D.sharedMaterial.bounciness = accumulatedForce;
			other.rigidbody2D.velocity = vel;
			bounce.Play ();
			accumulatedForce = 0;
//			}
			canBounce = false;
			timer = 0;
		}
	}
		
	protected virtual void OnCollisionStay2D (Collision2D col)
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
