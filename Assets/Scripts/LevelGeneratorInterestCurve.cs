using UnityEngine;
using System.Collections;

[System.Serializable]
public class Level
{
	public int scoreToBegin;
	public float horizontalForce = 3f;
	public float minSize = 3f;
	public float maxSize = 6f;

}

public class LevelGeneratorInterestCurve : LevelGenerator
{

	public float idealDistanceBetweenWalls = 6f;
	public Level[] levels;
	int currStage = 0;
	protected override void Start ()
	{
		base.Start ();
		if (score >= levels [currStage].scoreToBegin) {
			player.horizontalForce = levels [currStage].horizontalForce;
			AdjustBasedOnHorizonalForce (levels [currStage].horizontalForce);
			currStage++;
		}
	}
	public override void IncreaseScore ()
	{
		base.IncreaseScore ();
		if (currStage < levels.Length && score >= levels [currStage].scoreToBegin) {
			player.horizontalForce = levels [currStage].horizontalForce;
			minSize = levels [currStage].minSize;
			maxSize = levels [currStage].maxSize;
			AdjustBasedOnHorizonalForce (levels [currStage].horizontalForce);
			currStage++;
		}
	}
	
	protected float DetermineDistance (float currY)
	{
		float vi = Mathf.Sqrt (-2f * Physics2D.gravity.y * currY);
		float t = -vi / Physics2D.gravity.y;
		return player.horizontalForce * 2 * t;
	}
	void AdjustBasedOnHorizonalForce (float force)
	{
		if (force > 0) {
			float dummyY = 5f;
			float t = idealDistanceBetweenWalls / force;
			float vi = (dummyY / (t / 2)) * 2;
			player.gravity.y = (vi * vi) / (-2f * dummyY);
			Physics2D.gravity = player.gravity;
			//vf^2 = vi^2 + 2 * a * d
			player.maxForce = Mathf.Sqrt (-2f * player.gravity.y * topY);
			Debug.Log ("Max force: " + player.maxForce + ", Time: " + t + "Gravity: " + player.gravity.y);
			player.forceAddRate = Mathf.Max (0.35f, (player.maxForce / (t / 3.5f)) * Time.deltaTime);
			Debug.Log (player.forceAddRate);
		}
	}
}