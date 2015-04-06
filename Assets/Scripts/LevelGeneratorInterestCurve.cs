using UnityEngine;
using System.Collections;

[System.Serializable]
public class Level
{
	public int scoreToBegin;
	public float minSize = 3f;
	public float maxSize = 6f;
	public float incrementInTimeScale = 0.1f;
	public int levelsPerIncrement = 3;
	public float probabilityOfMovingObstacles = 0.3f;
	public float speedOfMovingObstacles = 0.1f;
	public float minPropOfSpaceToMove = 0.2f;
	public float maxPropOfSpaceToMove = 0.4f;
	

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
	public int scoreToKeepMarkTill = 6;
	public Transform mark;
	public Transform biggerMark;
	public float minForceScale = 0.2f;
	public int scoreOffset;
	public int noOfAddForceSteps = 7;
	public float probabilityOfHealth = 0.1f;
	public float probabilityOfInvul = 0.1f;
	public int maxNoOfLives = 3;
	public GameObject hearts, chargeTutorial, rightTutorial;
	public AudioSource gainLifeSound, fullHeartSound, loseHeartSound;
	bool hasPressedSpace, hasPressedRight;
	Object powerupLock = new Object ();
	//public float startTimeScale = 0.8f;

	int lives = 0;
	int currStage = 0;
	int scoreAtBallBounce = -1;
	Queue comingObstacles = new Queue ();

	protected override void Start ()
	{
	
		player = Player.instance;
		//Time.timeScale = startTimeScale;
		while (currStage < levels.Length - 1 && score + scoreOffset > levels [currStage+1].scoreToBegin) {
			player.horizontalForce += levels [currStage].incrementInTimeScale * 
				(int)(((levels [currStage + 1].scoreToBegin - levels [currStage].scoreToBegin) 
				/ levels [currStage].levelsPerIncrement) + 1);
			currStage++;
		}
		player.horizontalForce += levels [currStage].incrementInTimeScale * ((score + scoreOffset - levels [currStage].scoreToBegin) 
			/ levels [currStage].levelsPerIncrement - 1);
		//SetTimeScale ();

		SetMinMaxSize ();
		AdjustBasedOnHorizontalForce (player.horizontalForce);

//			player.horizontalForce = levels [currStage].horizontalForce;
//			AdjustBasedOnHorizontalForce (levels [currStage].horizontalForce);
		//SetPhysicsValues (valuesToUse);
		
		base.Start ();
		ObstacleSpecs next = (ObstacleSpecs)comingObstacles.Dequeue ();
		if (score + scoreOffset <= scoreToKeepMarkTill) {
			AdjustMark (next);
		} else {
			mark.gameObject.SetActive (false);
		}
		player.Start ();
		RenderForceBar ();
		if (!PlayerPrefs.HasKey ("PlayedBefore")) {
			StartCoroutine (CheckKeyPress (KeyCode.Space));
			StartCoroutine (CheckKeyPress (KeyCode.RightArrow));
			StartCoroutine (ChargeTutorial ());
		}
	}

	IEnumerator CheckKeyPress (KeyCode k)
	{
		while (!Input.GetKeyDown(k)) {
			yield return null;
		}
		if (k == KeyCode.Space) {
			hasPressedSpace = true;
		} else {
			hasPressedRight = true;
		}
	}

	IEnumerator ChargeTutorial ()
	{
		yield return new WaitForSeconds (0.8f);
		if (!hasPressedSpace) {
			chargeTutorial.transform.GetChild (0).gameObject.SetActive (true);
			Time.timeScale = 0;
			while (!Input.GetKeyDown (KeyCode.Space)) {
				yield return null;
			}
			chargeTutorial.transform.GetChild (0).gameObject.SetActive (false);
			chargeTutorial.transform.GetChild (1).gameObject.SetActive (true);
			yield return null;
			while (!Input.GetKey (KeyCode.Space)) {
				yield return null;
			}
			hasPressedSpace = true;
			Time.timeScale = 1;
			yield return new WaitForSeconds (0.5f);
			chargeTutorial.SetActive (false);
			StartCoroutine (RightTutorial (0.7f));
		} else {
			StartCoroutine (RightTutorial (1.5f));
		}
		PlayerPrefs.SetInt ("PlayedBefore", 1);

	}

	IEnumerator RightTutorial (float time)
	{
		yield return new WaitForSeconds (time);
		if (!hasPressedRight) {
			rightTutorial.SetActive (true);
			Time.timeScale = 0;
		
			while (!Input.GetKey(KeyCode.RightArrow)) {
				yield return null;
			}
			hasPressedRight = true;
			Time.timeScale = 1;
			rightTutorial.SetActive (false);
		}
		PlayerPrefs.SetInt ("PlayedBefore", 1);
	}

//	void SetTimeScale ()
//	{
//		Time.timeScale += levels [currStage].incrementInTimeScale;
//		Debug.Log ((score + scoreOffset) + ": " + Time.timeScale);
//		Time.fixedDeltaTime = 0.02f * Time.timeScale;
//	}

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
				if (currStage < levels.Length - 1 && score + scoreOffset >= levels [currStage + 1].scoreToBegin) {
					currStage++;
					SetMinMaxSize ();
				}
				if ((score + scoreOffset - levels [currStage].scoreToBegin) % levels [currStage].levelsPerIncrement == 0) {
					//SetTimeScale ();
					player.horizontalForce += levels [currStage].incrementInTimeScale;
					AdjustBasedOnHorizontalForce (player.horizontalForce);
					//SetPhysicsValues (valuesToUse);	
				}
			}
			if (score + scoreOffset <= scoreToKeepMarkTill) {
				AdjustMark (next);
				if (score + scoreOffset == scoreToKeepMarkTill - 1) {
					//StartCoroutine (Blink ());
					mark.GetComponent<Animator> ().SetBool ("Blink", true);
				} else if (score + scoreOffset == scoreToKeepMarkTill) {
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
	public override void DecreasePowerUps ()
	{
		lock (powerupLock) {
			powerUpsApplied--;
		}
	}

	public override void Restart (Collider2D col, bool resetPos)
	{

		Restart (col);
		Vector3 pos = col.transform.position;
		pos.y = topY;
		pos.x = player.transform.position.x;
		col.transform.position = pos;
		col.rigidbody2D.velocity = Vector3.zero;
	}

	public override void Restart (Collider2D col)
	{
		if (lives == 0) {
			base.Restart (col);
		} else {
			lives--;
			DecreasePowerUps ();
			loseHeartSound.Play ();
			if (lives == 0) {
				StartCoroutine (WaitAWhile ());
			}
			//hearts.transform.GetChild (lives).GetComponent<Animator> ().SetTrigger ("Break");
			StartCoroutine (BreakHeart (lives));

		}
	}
	IEnumerator WaitAWhile ()
	{
		yield return new WaitForSeconds (0.4f);
		DecreasePowerUps ();
		if (powerUpsApplied == 0) {
			Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("PlayerCollider"), LayerMask.NameToLayer ("Obstacle"), false);
		}
	}
	IEnumerator BreakHeart (int index)
	{
		hearts.transform.GetChild (index).GetComponent<Animator> ().SetTrigger ("Break");
		yield return new WaitForSeconds (0.5f);
		hearts.transform.GetChild (index).gameObject.SetActive (false);
		
	}

	public override void GainLife ()
	{
		if (lives < maxNoOfLives) {
			hearts.transform.GetChild (lives).gameObject.SetActive (true);
			hearts.transform.GetChild (lives).GetComponent<Animator> ().SetTrigger ("Pulse");
			lives ++;
			gainLifeSound.Play ();
		} else {
			for (int i =0; i<lives; i++) {
				hearts.transform.GetChild (i).GetComponent<Animator> ().SetTrigger ("Pulse");
			}
			fullHeartSound.Play ();
		}
		
	}

	public void AdjustMark (ObstacleSpecs specs)
	{
		float d = specs.posOfSpace.y;

		float vi = Mathf.Sqrt (-2 * Physics2D.gravity.y * d);
		mark.localPosition = new Vector3 (minForceScale + ((vi - player.defaultMinForce) / (player.maxForce - player.defaultMinForce)) * (1 - minForceScale), 0, 0);
		//mark.localScale = new Vector3 (minForceScale + ((vi - player.defaultMinForce) / (player.maxForce - player.defaultMinForce)) * (1 - minForceScale), 1, 1);
//		float smallestVi = Mathf.Sqrt (-2 * Physics2D.gravity.y * (Mathf.Max (botY + minYOffset, (d - specs.sizeOfSpace / 2 + 0.8f))));
//		float biggestVi = Mathf.Sqrt (-2 * Physics2D.gravity.y * Mathf.Min (topY, (d + specs.sizeOfSpace / 2 - 0.8f)));
//		biggerMark.localScale = new Vector3 ((biggestVi - smallestVi) / player.maxForce, 1, 1);
//		biggerMark.localPosition = mark.localPosition;
	}

	public override void RenderForceBar ()
	{
		Vector3 scale = player.forceBar.transform.localScale;
		scale.x = minForceScale + ((player.accumulatedForce - player.defaultMinForce) / (player.maxForce - player.defaultMinForce)) * (1 - minForceScale);
		player.forceBar.transform.localScale = scale;
	}

	protected override void GenerateObstacles ()
	{
		float y = RandomizeCurrY ();
		float x = DetermineCurrX (y);
		ObstacleSpecs obs = new ObstacleSpecs (new Vector3 (x, y), RandomizeDistance ());
		if (Random.value < levels [currStage].probabilityOfMovingObstacles) {
			obs.type = ObstacleSpecs.Types.Moving;
		}
		if (Random.value < probabilityOfInvul) {
			invulPool.Spawn (GetSpotForPowerUp (obs));
		} else if (Random.value < probabilityOfHealth) {
			healthPool.Spawn (GetSpotForPowerUp (obs));
		}
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
			//Debug.Log ((score + scoreOffset) + ": " + force);
			float dummyY = 5f;
			float t = idealDistanceBetweenWalls / force;
			float vi = (dummyY / (t / 2)) * 2;
			player.gravity.y = (vi * vi) / (-2f * dummyY);

			//	return new PhysicsValues (player.gravity.y, Mathf.Sqrt (-2f * player.gravity.y * (minYOffset + minSize / 2f)), Mathf.Sqrt (-2f * player.gravity.y * topY), ((player.maxForce - player.defaultMinForce) / (t / 3f)) * Time.deltaTime);
			Physics2D.gravity = player.gravity;
//			float vi = Mathf.Sqrt (-2f * Physics2D.gravity.y * dummyY);
//			float t = -vi / Physics2D.gravity.y;
			player.maxForce = Mathf.Sqrt (-2f * player.gravity.y * topY);
			player.defaultMinForce = Mathf.Sqrt (-2f * player.gravity.y * (minYOffset));
			player.forceAddRate = ((player.maxForce - player.defaultMinForce) / (t / 3.5f)) * Time.deltaTime;
//			player.forceAddRate = ((player.maxForce - player.defaultMinForce) / (t / 3f)) * (1f / 8f);
//			player.forceAddRate = (player.maxForce - player.defaultMinForce) / noOfAddForceSteps;

		} else {
			throw new UnityException ();
		}
	}
	Vector3 GetSpotForPowerUp (ObstacleSpecs specs)
	{
		float x = specs.posOfSpace.x + Random.Range (0, 5f);
		float y = Random.Range (2f, specs.posOfSpace.y + specs.sizeOfSpace - 3f);
		return new Vector3 (x, y, 0);
	}
	void SetMinMaxSize ()
	{
		minSize = levels [currStage].minSize;
		maxSize = levels [currStage].maxSize;

	}
	protected override void RenderObstacles (ObstacleSpecs specs)
	{
		if (specs.type == ObstacleSpecs.Types.Normal) {
			base.RenderObstacles (specs);
		} else if (specs.type == ObstacleSpecs.Types.Moving) {
		
			Color c = colorsForObstacles [Random.Range (0, colorsForObstacles.Length)];
			while (c == prevColor) {
				c = colorsForObstacles [Random.Range (0, colorsForObstacles.Length)];
			}
			float amtToMove = Random.Range (specs.sizeOfSpace * 0.2f, specs.sizeOfSpace * 0.4f);
			
			float topScaleY = topY - (specs.posOfSpace.y + specs.sizeOfSpace / 2) + amtToMove;
			if (topScaleY - amtToMove > minTopScaleY) {
				Vector3 topPos = specs.posOfSpace;
				topPos.y = topY;
				GameObject pillar = movingObstacles.Spawn (topPos, topScaleY);
				pillar.GetComponent<MovingObstacle> ().moveAmt = amtToMove;
				pillar.GetComponent<SpriteRenderer> ().color = c;
			}
			
			float botScaleY = (specs.posOfSpace.y - specs.sizeOfSpace / 2) - botY + amtToMove;
			Vector3 botPos = specs.posOfSpace;
			botPos.y -= specs.sizeOfSpace / 2 + amtToMove;
			GameObject pillar2 = movingObstacles.Spawn (botPos, botScaleY);
			pillar2.GetComponent<MovingObstacle> ().moveAmt = amtToMove;
			pillar2.GetComponent<SpriteRenderer> ().color = c;
			
			Vector3 pos = specs.posOfSpace; 
			pos.y = botY + (topY - botY) / 2;
			pointTriggers.Spawn (pos, topY - botY);
			prevColor = c;
		}
	}	

}