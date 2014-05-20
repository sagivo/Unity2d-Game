using UnityEngine;
using System.Collections;

public class Bullet : Liveable {
	public float Speed = 100;

	void Start () {
		rigidbody2D.velocity = transform.forward * Speed;
		OnHit += (o) => {
			if((o as Collider2D).gameObject.tag == "Wall") DestroyObject(this.gameObject);
		};
	}

	void OnBecameInvisible () {
		DestroyObject(this);
	}
}
