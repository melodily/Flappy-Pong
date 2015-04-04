using UnityEngine;
using System.Collections;

public class PlayerFollowBall : Player
{
	
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
