using UnityEngine;
using System.Collections;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
	protected static T instance;
	public static T Instance { get { return instance; } }
	protected void Awake ()
	{
		if (Singleton<T>.Instance != null) {
			if (instance != this) {
				Debug.Log ("Multiple instances of " + typeof(T).ToString () + " deteced.");
				Destroy (this.gameObject);
				return;
			}						
		} else {
			Singleton<T>.instance = this as T;
			
			AwakeProtected ();
		}
	}
	
	protected virtual void AwakeProtected ()
	{
	}
}
