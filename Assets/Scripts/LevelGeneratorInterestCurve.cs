using UnityEngine;
using System.Collections;

[System.Serializable]
public class Level
{
	public int scoreToBegin;
	//public float horizontalForce = 3f;
	public float timeScale = 1f;
	public float minSize = 3f;
	public float maxSize = 6f;

}

[System.Serializable]
public class PowerUpLevel
{
	public int minScore = 9;
	public float probability = 0.1f;
	public PowerUp.Types type;

}
//public class PhysicsValues
//{
//	public float gravity, minForce, maxForce, forceAddRate;
//	public PhysicsValues (float g, float min, float max, float add)
//	{
//		gravity = g;
//		minForce = min;
//		maxForce = max;
//		forceAddRate = add;
//	}
//}

public class LevelGeneratorInterestCurve : LevelGenerator
{

	public float idealDistanceBetweenWalls = 6f;
	public Level[] levels;
	public PowerUp[] powerups;
	public int scoreToKeepMarkTill = 6;
	public Transform mark;
	public Transform biggerMark;
	public float minForceScale = 0.2f;
	int currStage = 0;
	int scoreAtBallBounce = -1;
	Queue comingObstacles = new Queue ();
	protected override void Start ()
	{
		player = Player.instance;
		if (score >= levels [currStage].scoreToBegin) {
			SetTimeScale ();

			SetMinMaxSize ();
			AdjustBasedOnHorizontalForce (player.horizontalForce);

//			player.horizontalForce = levels [currStage].horizontalForce;
//			AdjustBasedOnHorizontalForce (levels [currStage].horizontalForce);
			//SetPhysicsValues (valuesToUse);
			currStage++;
		}
		base.Start ();
		if (score == 0) {
			ObstacleSpecs next = (ObstacleSpecs)comingObstacles.Dequeue ();
			if (score <= scoreToKeepMarkTill) {
				AdjustMark (next);
			}
		}
		Debug.Log (Time.timeScale);
	}

	void SetTimeScale ()
	{
		Time.timeScale = levels [currStage].timeScale;
		Time.fixedDeltaTime = 0.02f * Time.timeScale;
		//player.speed = Time.timeScale 
	}

//	void SetPhysicsValues (PhysicsValues v)
//	{
//		Physics2D.gravity = new Vector3 (0, v.gravity, 0);
//		player.maxForce = v.maxForce;
//		player.defaultMinForce = v.minForce;
//		player.forceAddRate = v.forceAddRate;
//		Debug.Log ("Max force: " + v.maxForce + " Gravity: " + v.gravity + " Force add rate: " + v.forceAddRate);
//	}
//	public override void IncreaseScore ()
//	{
//		base.IncreaseScore ();
//
//		if (currStage < levels.Length) {
//			if (score >= levels [currStage].scoreToBegin) {
//				player.horizontalForce = levels [currStage].horizontalForce;
//				minSize = levels [currStage].minSize;
//				maxSize = levels [currStage].maxSize;
//				//SetPhysicsValues (valuesToUse);
//				currStage++;
//			}
//		}
//
//	}
	public override void CallbackAfterBallBounce ()
	{
		if (score != scoreAtBallBounce) {
			ObstacleSpecs next = (ObstacleSpecs)comingObstacles.Dequeue ();

			if (currStage < levels.Length) {
				if (score >= levels [currStage].scoreToBegin) {
					SetTimeScale ();
					//AdjustBasedOnHorizontalForce (levels [currStage].horizontalForce);
					//player.horizontalForce = levels [currStage].horizontalForce;
					SetMinMaxSize ();
					//SetPhysicsValues (valuesToUse);
					currStage++;
				}
			}
			if (score <= scoreToKeepMarkTill) {
				AdjustMark (next);
				if (score == scoreToKeepMarkTill - 1) {
					//StartCoroutine (Blink ());
					mark.GetComponent<Animator> ().SetBool ("Blink", true);
				} else if (score == scoreToKeepMarkTill) {
					mark.gameObject.SetActive (false);
				}
//			} else {
//				if (mark.gameObject.activeSelf) {
//					mark.gameObject.SetActive (false);
//					biggerMark.gameObject.SetActive (false);
//				}
			}
			scoreAtBallBounce = score;
		}
		
	}

	public void AdjustMark (ObstacleSpecs specs)
	{
		float d = specs.posOfSpace.y;

		float vi = Mathf.Sqrt (-2 * Physics2D.gravity.y * d);
		//mark.localPosition = new Vector3 ((vi / player.maxForce), 0, 0);
		mark.localScale = new Vector3 (minForceScale + ((vi - player.defaultMinForce) / (player.maxForce - player.defaultMinForce)) * (1 - minForceScale), 1, 1);
//		float smallestVi = Mathf.Sqrt (-2 * Physics2D.gravity.y * (Mathf.Max (botY + minYOffset, (d - specs.sizeOfSpace / 2 + 0.8f))));
//		float biggestVi = Mathf.Sqrt (-2 * Physics2D.gravity.y * Mathf.Min (topY, (d + specs.sizeOfSpace / 2 - 0.8f)));
//		biggerMark.localScale = new Vector3 ((biggestVi - smallestVi) / player.maxForce, 1, 1);
//		biggerMark.localPosition = mark.localPosition;
	}

	public override void RenderForceBar ()
	{
		Vector3 scale = player.forceBar.transform.localScale;
		scale.x = minForceScale + ((player.accumulatedForce - player.defaultMinForce) / (player.maxForce - player.defaultMinForce)) * (1 - minForceScale);
		//Debug.Log(valuesToUse.forceAddRate)
		player.forceBar.transform.localScale = scale;
	}


	protected override void GenerateObstacles ()
	{
		float y = RandomizeCurrY ();
		float x = DetermineCurrX (y);
		ObstacleSpecs obs = new ObstacleSpecs (new Vector3 (x, y), RandomizeDistance ());
		comingObstacles.Enqueue (obs);
		RenderObstacles (obs);
	}
	protected float DetermineDistance (float currY)
	{
		float vi = Mathf.Sqrt (-2f * Physics2D.gravity.y * currY);
		float t = -vi / Physics2D.gravity.y;
		return player.horizontalForce * 2 * t;
	}
	void AdjustBasedOnHorizontalForce (float force)
	{
		if (force > 0) {
			float dummyY = 5f;
			float t = idealDistanceBetweenWalls / force;
			float vi = (dummyY / (t / 2)) * 2;
			player.gravity.y = (vi * vi) / (-2f * dummyY);

			//	return new PhysicsValues (player.gravity.y, Mathf.Sqrt (-2f * player.gravity.y * (minYOffset + minSize / 2f)), Mathf.Sqrt (-2f * player.gravity.y * topY), ((player.maxForce - player.defaultMinForce) / (t / 3f)) * Time.deltaTime);
			Physics2D.gravity = player.gravity;
			player.maxForce = Mathf.Sqrt (-2f * player.gravity.y * topY);
			player.forceAddRate = ((player.maxForce - player.defaultMinForce) / (t / 3f)) * Time.deltaTime;
			player.defaultMinForce = Mathf.Sqrt (-2f * player.gravity.y * (minYOffset));

		} else {
			throw new UnityException ();
		}
	}

	void SetMinMaxSize ()
	{
		minSize = levels [currStage].minSize;
		maxSize = levels [currStage].maxSize;

	}
}