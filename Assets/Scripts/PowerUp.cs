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
	public float time = 6f;
	void OnCollisionEnter2D (Collision2D col)
	{
		Collider2D other = col.collider;
		if (other.tag == "Ball") {
			SetPowerUp (other);
		}
	}

	void SetPowerUp (Collider2D other)
	{
		if (type == Types.Invulnerable) {
			StartCoroutine (SetInvulnerable (other.GetComponent<Animator> ()));
		}
	}
	IEnumerator SetInvulnerable (Animator anim)
	{
		anim.SetBool ("Color", true);
		Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("Player"), LayerMask.NameToLayer ("Obstacle"), true);
		yield return new WaitForSeconds (0.8f * time);
		anim.SetBool ("Blink", true);
		anim.SetBool ("Color", false);
		yield return new WaitForSeconds (0.2f * time);
		anim.SetBool ("Blink", false);
		Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("Player"), LayerMask.NameToLayer ("Obstacle"), false);


	}
}
