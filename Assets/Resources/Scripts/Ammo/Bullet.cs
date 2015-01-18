using UnityEngine;
using System.Collections;

public class Bullet : BaseObj {
	public float speed = 2;

	protected new void Start(){
		base.Start();
		transform.parent = Game.spawnerLayer.transform;
		//rigidbody2D.AddForce (transform.right * speed);

	}

	protected new void Update(){
		base.Update();

		transform.Translate(Vector2.right * speed * Time.deltaTime);
	}
}
