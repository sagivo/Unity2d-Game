using UnityEngine;
using System.Collections;

public class Bullet : BaseObj {
	public float speed = 200;
	public int damage = 10;

	[System.NonSerialized]
	public System.Type[] hits;

	protected new void Start(){
		transform.parent = Game.spawnerLayer.transform;
		rigidbody2D.AddForce (transform.up * speed);
		rigidbody2D.velocity = transform.forward * speed;
	}

	void OnTriggerEnter2D(Collider2D other){
		if (hits==null) return;
		//if (hits!=null && System.Array.IndexOf(hits, other.gameObject.tag) > -1) {
		//		l("b");Destroy(gameObject); 
		//}
		foreach (System.Type t in hits) 
			if (other.GetComponent(t) != null)
				Destroy(gameObject);
	}
	
}
