using UnityEngine;
using System.Collections;

public class Bullet : Liveable {
	public float Speed = 500;
	public int Damage = 10;

	protected string[] hitTags = { "Bullet", "Enemy" };

	protected new void Awake(){
		ShowHealthBar = false;
	}

	protected new void Start(){
		base.Start();
		rigidbody2D.velocity = transform.forward * Speed;
		OnHit += (o) => {
			if (System.Array.IndexOf(hitTags, o.gameObject.tag) > -1) Destroy(gameObject);
		};

	}

	void OnBecameInvisible () {
		DestroyObject(this);
	}
}
