using UnityEngine;
using UnityEngine.UI;
using System.Collections;



public class ObstacleSpecs
{
	public Vector3 posOfSpace;
	public float sizeOfSpace;
	public ObstacleSpecs (Vector3 pos, float size)
	{
		posOfSpace = pos;
		sizeOfSpace = size;
	}
}

public class LevelGenerator : Singleton<LevelGenerator>
{

	public ObjectPooling obstacles;
	public ObjectPooling pointTriggers;
	protected Player player;
	public Transform ball;
	public float botY, topY;
	public float xExtentsFromCamera;
	public float maxSize = 6f;
	public float minSize = 3f;
	public float minYOffset = 2f;
	public float minTopScaleY = 1f;
	public Text scoreText;
	public Text gameoverScoreText;
	public Text highscoreText;
	public Color[] colorsForObstacles;
	[HideInInspector]
	public int
		score = 0;
	public static bool isGameStarted = false;
	public bool isGameOver = false;
	public GameObject startScreen, endScreen;
	public AudioSource point, die;
	float lastBallBounceLocation;
	Color prevColor = Color.white;
	
	
	protected virtual void Start ()
	{
		//startScreen.SetActive (!isGameStarted);
		player = Player.instance;
		lastBallBounceLocation = player.transform.position.x;
		highscoreText.text = "High Score: " + PlayerPrefs.GetInt ("Score").ToString ();
//		if (!isGameStarted) {
//			Time.timeScale = 0;
//		}
		StartGame ();
		Physics2D.gravity = player.gravity;
		GenerateObstacles ();
	}
	protected virtual void Update ()
	{
		if (!isGameStarted) {
			if (Input.GetKeyDown (KeyCode.Return)) {
				StartGame ();
			}
		} else if (isGameOver) {
			if (Input.anyKeyDown) {
				Application.LoadLevel (Application.loadedLevel);
				Time.timeScale = 1;
			}
		}
		while (lastBallBounceLocation < Camera.main.transform.position.x + xExtentsFromCamera) {
			GenerateObstacles ();
		}
	}
		
	void StartGame ()
	{
		isGameStarted = true;
		startScreen.SetActive (false);
		endScreen.SetActive (false);
	}
		
	public virtual void IncreaseScore ()
	{
		score++;
		point.Play ();
		scoreText.text = score.ToString ();
		player.canBounce = true;
	}
		
	public void Restart ()
	{
		if (score > PlayerPrefs.GetInt ("Score")) {
			PlayerPrefs.SetInt ("Score", score);
			highscoreText.text = "High Score: " + score.ToString ();
		}
		gameoverScoreText.text = "Score: " + score.ToString ();
		die.Play ();
		isGameOver = true;
		Time.timeScale = 0;
		endScreen.SetActive (true);
	}
	protected virtual void GenerateObstacles ()
	{
//				float y = RandomizeCurrY ();
//				float x = DetermineCurrX (y);
//				RenderObstacles (new ObstacleSpecs (new Vector3 (x, y), RandomizeDistance ()));
//				while (x< Camera.main.transform.position.x + xExtentsFromCamera) {
		//for (int i = 0; i<3; i++) {
		float y = RandomizeCurrY ();
		float x = DetermineCurrX (y);
		RenderObstacles (new ObstacleSpecs (new Vector3 (x, y), RandomizeDistance ()));
		//		}
	}
	protected float RandomizeCurrY ()
	{
		return Random.Range (minYOffset + minSize / 2f, topY - minSize / 2f);
	}
	protected float RandomizeDistance ()
	{
		return Random.Range (minSize, maxSize);
	}
	public virtual void RenderForceBar ()
	{
		Vector3 scale = player.forceBar.transform.localScale;
		scale.x = player.accumulatedForce / player.maxForce;
		player.forceBar.transform.localScale = scale;
	}
	public virtual void CallbackAfterBallBounce ()
	{

	}
	protected float DetermineCurrX (float currY)
	{
		float vi = Mathf.Sqrt (-2f * Physics2D.gravity.y * currY);
		float t = -vi / Physics2D.gravity.y;
		float prevX = lastBallBounceLocation;
		lastBallBounceLocation += player.horizontalForce * 2 * t;
		return prevX + player.horizontalForce * t;

	}

	protected void RenderObstacles (ObstacleSpecs specs)
	{
		float topScaleY = topY - (specs.posOfSpace.y + specs.sizeOfSpace / 2);
		Color c = colorsForObstacles [Random.Range (0, colorsForObstacles.Length)];
		while (c == prevColor) {
			c = colorsForObstacles [Random.Range (0, colorsForObstacles.Length)];
		}
		if (topScaleY > minTopScaleY) {
			Vector3 topPos = specs.posOfSpace;
			topPos.y = topY;
			GameObject pillar = obstacles.Spawn (topPos, topScaleY);
			pillar.GetComponent<SpriteRenderer> ().color = c;
		}
		float botScaleY = (specs.posOfSpace.y - specs.sizeOfSpace / 2) - botY;
		Vector3 botPos = specs.posOfSpace;
		botPos.y -= specs.sizeOfSpace / 2;
		GameObject pillar2 = obstacles.Spawn (botPos, botScaleY);
		pillar2.GetComponent<SpriteRenderer> ().color = c;
		Vector3 pos = specs.posOfSpace; 
		pos.y = botY + (topY - botY) / 2;
		pointTriggers.Spawn (pos, topY - botY);
		prevColor = c;
	}
	

}
