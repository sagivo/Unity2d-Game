using UnityEngine;
using System.Collections;

public class Hitable : BaseObj {
	[System.NonSerialized]
	public System.Type[] hits;
	[System.NonSerialized]
	public int damage;

	void OnTriggerEnter2D(Collider2D other){
		if (hits==null) return;
		foreach (System.Type t in hits) 
		if (other.GetComponent(t) != null) {Destroy(gameObject);break;}
	}
}
