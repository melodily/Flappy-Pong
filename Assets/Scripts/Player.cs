using UnityEngine;
using System.Collections;

public class Player : Singleton<Player>
{

	// Use this for initialization
	public float speed = 10f;
	public float forceAddRate = 0.6f;
	public float horizontalForce = 3f;
	public GameObject forceBar; 
	[HideInInspector]
	public float
		accumulatedForce = 0;
	public float maxForce = 13;
	[HideInInspector]
	public float
		defaultMinForce;
	public AudioSource bounce, charge;
	protected float maxTimeAllowedOnPaddle = 0.05f;
	protected float timer;
	public Vector3 gravity = new Vector3 (0, -9.81f, 0);
	[HideInInspector]
	public bool
		canBounce = true;

	public void Start ()
	{

		accumulatedForce = defaultMinForce;

	}
	void OnDestroy ()
	{
		Physics2D.gravity = new Vector3 (0, -9.81f, 0);
	}
		



	// Update is called once per frame
	protected virtual void Update ()
	{
		if (LevelGenerator.isGameStarted) {
			
			Move ();
			Charge ();
//			Vector3 pos = transform.position;
//			pos.x = LevelGenerator.Instance.ball.position.x;
//			transform.position = pos;

			LevelGenerator.instance.RenderForceBar ();
		}
	}
	protected virtual void Move ()
	{
		Vector3 vel = Vector3.zero;
		if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) {
			vel.x = -speed;
		}
		if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) {
			vel.x = speed;
		}	
		rigidbody2D.velocity = vel;
	}
	
	protected virtual void Charge ()
	{
		if ((Input.GetKey (KeyCode.Space)) || Input.GetMouseButton (0)) {
			//charge.Play ();
			accumulatedForce = Mathf.Min (accumulatedForce + forceAddRate, maxForce);
		}
	}
	protected virtual void OnTriggerEnter2D (Collider2D other)
	{
//		Collider2D other = col.collider;
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
			LevelGenerator.instance.CallbackAfterBallBounce ();
			ResetAccumulatedForce ();

//			}
			canBounce = false;
			timer = 0;
		}
	}
	
	protected virtual void ResetAccumulatedForce ()
	{
		accumulatedForce = defaultMinForce;
	}
		
	protected virtual void OnTriggerStay2D (Collider2D other)
	{
//		Collider2D other = col.collider;
		if (other.tag == "Ball") {
			timer += Time.deltaTime;
			if (timer > maxTimeAllowedOnPaddle) {
				LevelGenerator.Instance.Restart (other);
			}
		}
	}
		
}
