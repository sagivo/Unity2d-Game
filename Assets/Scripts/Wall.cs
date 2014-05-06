using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.transform.parent) Destroy(gameObject.transform.parent.gameObject);
		else Destroy(other.gameObject);
	}
}
