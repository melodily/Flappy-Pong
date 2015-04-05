using UnityEngine;
using System.Collections;

public class PlayerTap : Player
{

	protected override void Move ()
	{
		Vector3 vel = Vector3.zero;
		if (Input.GetKeyDown (KeyCode.LeftArrow) || Input.GetKeyDown (KeyCode.A)) {
			vel.x = -speed;
		}
		if (Input.GetKeyDown (KeyCode.RightArrow) || Input.GetKeyDown (KeyCode.D)) {
			vel.x = speed;
		}	
		transform.Translate (vel);

	}
	
	protected override void Charge ()
	{
		if ((Input.GetKeyDown (KeyCode.UpArrow) || Input.GetKeyDown (KeyCode.W) || Input.GetKeyDown (KeyCode.Space))) {
			//charge.Play ();
			//if (Input.GetKeyDown (KeyCode.Space)) {
			accumulatedForce = Mathf.Min (accumulatedForce + forceAddRate, maxForce);
		}
		if ((Input.GetKeyDown (KeyCode.DownArrow) || Input.GetKeyDown (KeyCode.S))) {
			accumulatedForce = Mathf.Max (accumulatedForce - forceAddRate, 0);
		}
	}
	
	protected override void ResetAccumulatedForce ()
	{
	
	}
	
}
