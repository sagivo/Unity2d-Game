using UnityEngine;
using System.Collections;

public class Bullet : BaseObj {
	public float Speed = 200;
	public int Damage = 10;

	protected string[] hitTags = { "Bullet", "Enemy" };

	protected new void Start(){
		L("created");
		base.Start();

		transform.parent = Game.spawnerLayer.transform;
		rigidbody2D.AddForce (transform.up * Speed);
		rigidbody2D.velocity = transform.forward * Speed;
	}

	void OnTriggerEnter2D(Collider2D other){
		if (System.Array.IndexOf(hitTags, other.gameObject.tag) > -1) Destroy(gameObject);
	}
	
}
