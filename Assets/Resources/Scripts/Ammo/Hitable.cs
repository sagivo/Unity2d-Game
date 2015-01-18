using UnityEngine;
using System.Collections;

public class Hitable : BaseObj {
	[System.NonSerialized]
	public System.Type[] hits;
	[System.NonSerialized]
	public int damage;
	public bool keepAlive;
	public GameObject destroyOnHit;

	void OnTriggerEnter2D(Collider2D other){
		if (hits==null || keepAlive) return;
		foreach (System.Type t in hits){
			if (other.GetComponent(t) != null) {
				l ("kill");
				if (destroyOnHit) Destroy(destroyOnHit);
				else Destroy(gameObject);break;
			}
		}
	}
}
