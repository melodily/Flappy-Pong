using UnityEngine;
using System.Collections;

public class PlayerFollowBall : Player
{
	public Vector3 gravity;
	void Start ()
	{
		Physics2D.gravity = gravity;
	}
	void OnDestroy ()
	{
		Physics2D.gravity = new Vector3 (0, -9.81f, 0);
	}
	// Update is called once per frame
	protected override void Update ()
	{
		if (LevelGenerator.isGameStarted) {
			Vector3 vel = Vector3.zero;
			if ((Input.GetKey (KeyCode.Space))) {
				accumulatedForce = Mathf.Min (accumulatedForce + forceAddRate, maxForce);
			}

//			rigidbody2D.velocity = vel;
			Vector3 pos = transform.position;
			pos.x = LevelGenerator.Instance.ball.position.x;
			transform.position = pos;
			Vector3 scale = forceBar.transform.localScale;
			scale.x = accumulatedForce / maxForce;
			forceBar.transform.localScale = scale;
		}
	}
	
}
