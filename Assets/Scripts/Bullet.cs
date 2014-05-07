using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	public float Speed = 100;
	// Use this for initialization
	void Start () {
		rigidbody2D.velocity = transform.forward * Speed;
		//rigidbody2D.AddForce(transform.forward * Speed);
	}
	
	// Update is called once per frame
	void Update () {
		//transform.position += transform.forward*Speed*Time.deltaTime;
	}

	void OnTriggerEnter2D(Collider2D other) {
		Destroy(gameObject);
	}
}
