using UnityEngine;
using System.Collections;

public class Bullet : Liveable {
	public float Speed = 100;

	void Start () {
		rigidbody2D.velocity = transform.forward * Speed;

	}

}
