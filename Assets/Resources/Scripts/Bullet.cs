using UnityEngine;
using System.Collections;

public class Bullet : BaseObj {
	public float speed = 200;
	public int damage = 10;

	protected string[] hitTags = { "Bullet", "Enemy" };

	protected new void Start(){
		L ("helo");
		base.Start();

		transform.parent = Game.spawnerLayer.transform;
		rigidbody2D.AddForce (transform.up * speed);
		rigidbody2D.velocity = transform.forward * speed;
	}

	void OnTriggerEnter2D(Collider2D other){
		if (System.Array.IndexOf(hitTags, other.gameObject.tag) > -1) Destroy(gameObject);
	}
	
}
