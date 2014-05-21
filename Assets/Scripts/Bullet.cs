using UnityEngine;
using System.Collections;

public class Bullet : Liveable {
	public float Speed = 500;
	public int Damage = 10;

	string[] hitTags = { "Bullet", "Enemy" };


	void Start () {
		rigidbody2D.velocity = transform.forward * Speed;
		OnHit += (o) => {
			if (System.Array.IndexOf(hitTags, o.gameObject.tag) > -1) Destroy(gameObject);
		};

	}

	void OnBecameInvisible () {
		DestroyObject(this);
	}
}
