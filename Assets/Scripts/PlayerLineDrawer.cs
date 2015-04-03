using UnityEngine;
using System.Collections;

public class PlayerLineDrawer : Player
{
	public LineRenderer lineRenderer;
	public float timeToRender = 0.5f;
	float lineTimer = 0;

	protected override void Update ()
	{
		base.Update ();
		if (Input.GetKey (KeyCode.Space)) {
			UpdateTrajectory ();
		}
		if (lineTimer > 0) {
			lineTimer -= Time.deltaTime;
			if (lineTimer <= 0) {
				Debug.Log ("Disappearing");
				lineRenderer.gameObject.SetActive (false);
			}
		}
	}
	void UpdateTrajectory ()
	{
		lineRenderer.gameObject.SetActive (true);
		float x = transform.position.x;
		float y = transform.position.y;
		float vi = accumulatedForce;
		float timeStep = 0.1f;
		float endY = y;
		float prevX = 0;
		float prevY = 0;
		ArrayList allPositions = new ArrayList ();
		allPositions.Add (new Vector3 (x, y, 0));
		while (y>=endY) {
			prevX = x;
			prevY = y;
			x += horizontalForce * timeStep;
			float vf = vi + timeStep * Physics2D.gravity.y;
			y += ((vf + vi) / 2) * timeStep;
			vi = vf;
			allPositions.Add (new Vector3 (x, y, 0));
		}
		lineRenderer.SetVertexCount (allPositions.Count);
		for (int i = 0; i<allPositions.Count; i++) {
			lineRenderer.SetPosition (i, (Vector3)allPositions [i]);
		}
		lineTimer = timeToRender;
	}
}
