using UnityEngine;
using System.Collections;

public class ObjectPooling : MonoBehaviour
{
	public GameObject prefab;
//		ArrayList availableObjects = new ArrayList ();
	Queue availableObjects = new Queue ();
//		public ObjectPooling (GameObject obj)
//		{
//				prefab = obj;
//		}

	void Start ()
	{
		for (int i =0; i<transform.childCount; i++) {
			availableObjects.Enqueue (transform.GetChild (i).gameObject);
		}
	}

	public GameObject Spawn (Vector3 pos, float yScale)
	{
		GameObject obj; 
		if (availableObjects.Count > 0) {
			obj = (GameObject)availableObjects.Dequeue ();
						
		} else {
			Debug.Log ("Instantiating " + prefab.name);
			obj = (GameObject)Object.Instantiate (prefab); 
			obj.transform.parent = transform;
		}

		obj.transform.position = pos;
		Vector3 scale = obj.transform.localScale;
		scale.y = yScale;
		obj.transform.localScale = scale;

		obj.SetActive (true);
		return obj;
	}

	public void Remove (GameObject obj)
	{
		availableObjects.Enqueue (obj);
		obj.gameObject.SetActive (false);
	}

	
}
