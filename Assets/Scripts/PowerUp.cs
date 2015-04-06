using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour
{
	public enum Types
	{
		Invulnerable,
		Life
	}
	public Types type = Types.Invulnerable;
	public float effectTime = 6f;
	public bool taken = false;
	static bool blink = false;
	static int invulsApplied = 0;
	static int blinksApplied = 0;
	Object invulLock = new Object ();
	Object blinksLock = new Object ();
	Animator anim;
	
	void Start ()
	{
		invulsApplied = 0;
		taken = false;
		blink = false;
	}
	
	void OnEnable ()
	{
		GetComponent<SpriteRenderer> ().enabled = true;
		taken = false;
		blink = false;
	}
	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Ball" && !taken) {
			SetPowerUp (other);
			taken = true;
		}
	}

	void SetPowerUp (Collider2D other)
	{
		if (type == Types.Invulnerable) {
			invulsApplied ++;
			blinksApplied++;
			
			StartCoroutine (SetInvulnerable (other.GetComponent<Animator> ()));
			
		} else if (type == Types.Life) {
			Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("PlayerCollider"), LayerMask.NameToLayer ("Obstacle"), true);
			LevelGeneratorInterestCurve.Instance.GainLife ();
			LevelGenerator.Instance.healthPool.Remove (gameObject);
			//gameObject.SetActive (false);
		}
		LevelGenerator.Instance.powerUpsApplied++;
		Debug.Log ("powerup " + LevelGenerator.Instance.powerUpsApplied);
	}
	
//	void Update ()
//	{
//		if (timer > 0 && type == Types.Invulnerable && taken) {
//			if (anim != null) {
//				timer -= Time.deltaTime;
//				if (!blink && timer <= 0.8f * effectTime) {
//					Debug.Log ("blink");
//					anim.SetTrigger ("Blink");
//					blink = true;
//				} 
//				if (timer <= 0) {
//					Debug.Log ("white");
//					anim.SetTrigger ("White");
//					powerupsApplied--;
//					Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("Player"), LayerMask.NameToLayer ("Obstacle"), false);
//					if (powerupsApplied == 0) {
//						Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("PlayerCollider"), LayerMask.NameToLayer ("Obstacle"), false);
//					}
//					timer = 0;	
//					LevelGenerator.Instance.invulPool.Remove (gameObject);
//				}
//			} else {
//				Debug.LogError ("Null anim");
//			}
//		} 
//	}
	IEnumerator SetInvulnerable (Animator anim)
	{
		GetComponentInParent<AudioSource> ().Play ();
		GetComponent<SpriteRenderer> ().enabled = false;
		anim.SetTrigger ("Color");
		Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("Player"), LayerMask.NameToLayer ("Obstacle"), true);
		Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("PlayerCollider"), LayerMask.NameToLayer ("Obstacle"), true);
		yield return new WaitForSeconds ((0.8f * effectTime));
		//timer -= (0.8f * effectTime) + 0.01f;
		lock (blinksLock) {
			blinksApplied--;
			if (blinksApplied <= 0) {
				anim.SetTrigger ("Blink");
				blinksApplied = 0;
			} else {
				Debug.Log ("Blinks applied " + blinksApplied);
			}
		}
		yield return new WaitForSeconds (0.2f * effectTime);
		lock (invulLock) {
			invulsApplied --;
			if (invulsApplied <= 0) {
				invulsApplied = 0;
				Debug.Log ("Gone");
				anim.SetTrigger ("White");
				Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("Player"), LayerMask.NameToLayer ("Obstacle"), false);
				LevelGenerator.Instance.DecreasePowerUps ();
				if (LevelGenerator.Instance.powerUpsApplied <= 0) {
					Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("PlayerCollider"), LayerMask.NameToLayer ("Obstacle"), false);

				}
				LevelGenerator.Instance.invulPool.Remove (gameObject);
			} else {
				Debug.Log ("Invuls " + invulsApplied);
			}
		}
	}
}
