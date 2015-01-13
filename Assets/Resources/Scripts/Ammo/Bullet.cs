using UnityEngine;
using System.Collections;

public class Bullet : BaseObj {
	public float speed = 200;
	[ExecuteInEditMode]

	protected new void Start(){
		transform.parent = Game.spawnerLayer.transform;
		rigidbody2D.AddForce (transform.right * speed);
	}

}
