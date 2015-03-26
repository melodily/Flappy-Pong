using UnityEngine;
using UnityEngine.UI;
using System.Collections;



class ObstacleSpecs
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
		public Player player;
		public Transform ball;
		public float botY, topY;
		public float xExtentsFromCamera;
		public float maxSize = 6f;
		public float minSize = 3f;
		public float minYOffset = 2f;
		public Text scoreText;
		public Text highscoreText;
		[HideInInspector]
		public int
				score = 0;
		public static bool isGameStarted = false;
		public bool isGameOver = false;
		public GameObject startScreen, endScreen;
		float lastBallBounceLocation;
	
	
		void Start ()
		{
				startScreen.SetActive (!isGameStarted);
				lastBallBounceLocation = player.transform.position.x;
				highscoreText.text = "High Score: " + PlayerPrefs.GetInt ("Score").ToString ();
				if (!isGameStarted) {
						Time.timeScale = 0;
				}
		}
		void Update ()
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
				Time.timeScale = 1;
				startScreen.SetActive (false);
				endScreen.SetActive (false);
		}
		
		public void IncreaseScore ()
		{
				score++;
				scoreText.text = score.ToString ();
				player.canBounce = true;
		}
		
		public void Restart ()
		{
				if (score > PlayerPrefs.GetInt ("Score")) {
						PlayerPrefs.SetInt ("Score", score);
						highscoreText.text = "High Score: " + score.ToString ();
				}
				isGameOver = true;
				Time.timeScale = 0;
				endScreen.SetActive (true);
		}
		void GenerateObstacles ()
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
		float RandomizeCurrY ()
		{
				return Random.Range (minYOffset + minSize / 2f, topY - minSize / 2f);
		}
		float RandomizeDistance ()
		{
				return Random.Range (minSize, maxSize);
		}

		float DetermineCurrX (float currY)
		{
				float vi = Mathf.Sqrt (-2f * Physics2D.gravity.y * currY);
				float t = -vi / Physics2D.gravity.y;
				float prevX = lastBallBounceLocation;
				lastBallBounceLocation += player.horizontalForce * 2 * t;
				return prevX + player.horizontalForce * t;

		}

		void RenderObstacles (ObstacleSpecs specs)
		{
				float topScaleY = topY - (specs.posOfSpace.y + specs.sizeOfSpace / 2);
				Vector3 topPos = specs.posOfSpace;
				topPos.y = topY;
				obstacles.Spawn (topPos, topScaleY);
				float botScaleY = (specs.posOfSpace.y - specs.sizeOfSpace / 2) - botY;
				Vector3 botPos = specs.posOfSpace;
				botPos.y -= specs.sizeOfSpace / 2;
				obstacles.Spawn (botPos, botScaleY);
				pointTriggers.Spawn (specs.posOfSpace, specs.sizeOfSpace);
		}
	

}
