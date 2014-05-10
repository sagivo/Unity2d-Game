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
		if(other.gameObject.tag == "WALL")
		{
			DestroyObject(this.gameObject);
		}
	}
}
