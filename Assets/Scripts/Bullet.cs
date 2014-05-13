using UnityEngine;
using System.Collections;

public class Bullet : Liveable {
	public float Speed = 100;

	void Start () {
		rigidbody2D.velocity = transform.forward * Speed;
	}

	void OnBecameInvisible () {
		DestroyObject(this);
	}

	void OnTriggerEnter2D(Collider2D other) {

		//when the bullet hits the walls of end of game area. Delete the bullet
		if(other.gameObject.tag == "Wall")
		{
			DestroyObject(this.gameObject);
		}
	}
}
