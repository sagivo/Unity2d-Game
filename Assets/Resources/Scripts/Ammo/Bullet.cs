using UnityEngine;
using System.Collections;

public class Bullet : BaseObj {
	public float speed = 2;
	[ExecuteInEditMode]

	protected new void Start(){
		base.Start();
		transform.parent = Game.spawnerLayer.transform;
		//rigidbody2D.AddForce (transform.right * speed);
		//rigidbody2D.MovePosition( + speed * Time.deltaTime);
		//Vector2 newPosition = rigidbody2D.position + speed * Time.deltaTime;
		//position = rigidbody2D.position + speed * Time.fixedDeltaTime;
		//rigidbody2D.MovePosition( newPosition);
		//transform.Translate(

	}

	protected new void Update(){
		base.Update();

		transform.Translate(Vector2.right * speed * Time.deltaTime);
		//transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
			//Vector2.MoveTowards(transform.position, Vector2.right, speed*Time.deltaTime);
	}
}
