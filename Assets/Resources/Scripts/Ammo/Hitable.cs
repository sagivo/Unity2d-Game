using UnityEngine;
using System.Collections;

public class Hitable : BaseObj {
	[System.NonSerialized]
	public System.Type[] hits;
	[System.NonSerialized]
	public int damage;
	public bool keepAlive;

	void OnTriggerEnter2D(Collider2D other){
		if (hits==null || keepAlive) return;
		foreach (System.Type t in hits) 
			if (other.GetComponent(t) != null) {Destroy(gameObject);break;}
	}
}
