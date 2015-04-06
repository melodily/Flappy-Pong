using UnityEngine;
using System.Collections;

public class PlayerTap : Player
{


	protected override void Move ()
	{
		base.Move ();
//		Vector3 pos = transform.position;
//		pos.x = LevelGenerator.instance.ball.position.x;
//		transform.position = pos;
	}
//	protected override void Charge ()
//	{
//		if (Input.GetKey (KeyCode.Space)) {
//			accumulatedForce = Mathf.Min (accumulatedForce + forceAddRate, maxForce);
//			if (!charge.isPlaying) {
//				charge.Play ();
//			}
//		} else {
//			charge.Stop ();
//		}
//	}
//	protected override void Charge ()
//	{
//		if ((Input.GetKeyDown (KeyCode.UpArrow) || Input.GetKeyDown (KeyCode.W) || Input.GetKeyDown (KeyCode.Space))) {
//			//charge.Play ();
//			//if (Input.GetKeyDown (KeyCode.Space)) {
//			accumulatedForce = Mathf.Min (accumulatedForce + forceAddRate, maxForce);
//		}
//	}
	
//	protected override void ResetAccumulatedForce ()
//	{
//	
//	}
	
}
