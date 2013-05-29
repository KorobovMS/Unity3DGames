using UnityEngine;
using System.Collections;

public class CollisionProcessing : MonoBehaviour {
	// Система частиц пламени
	public GameObject destructedObjectParticles;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnCollisionEnter(Collision collision) {
		GameObject explosion = Instantiate (destructedObjectParticles, 
			collision.gameObject.transform.position,
			Quaternion.identity) as GameObject;
		
		Destroy(gameObject);
		Destroy(collision.gameObject);
	}
}
