using UnityEngine;
using System.Collections;

public class PlayerCharge : PlayerLineDrawer
{
	protected bool shouldDecrement = false;
	protected override void Update ()
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
				if (!shouldDecrement) {
					if (accumulatedForce < maxForce) {
						accumulatedForce = Mathf.Min (accumulatedForce + forceAddRate, maxForce);
					} else {
						shouldDecrement = true;
					}
				}
				if (shouldDecrement) {
					if (accumulatedForce > 0) {
						accumulatedForce = Mathf.Max (accumulatedForce - forceAddRate, 0);
					} else {
						shouldDecrement = false;
					}
				}
			}
			rigidbody2D.velocity = vel;
			Vector3 scale = forceBar.transform.localScale;
			scale.x = accumulatedForce / maxForce;
			forceBar.transform.localScale = scale;
		}
	
	}
	protected override void OnCollisionEnter2D (Collision2D col)
	{
		base.OnCollisionEnter2D (col);
		Collider2D other = col.collider;
		if (other.tag == "Ball") {
			shouldDecrement = false;
		}
	}
}
