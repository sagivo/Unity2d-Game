using UnityEngine;
using System.Collections;

public class Kamikazi : Enemy {

	void Start () {
		findTarget();
		OnHit += (o) => {
			var hited = (o as Collider2D).gameObject;
			if (hited.IsSubClassOf<BaseObj>()) Destroy(hited);
		};
	}
	 
	void Update () {
		if (Target != null){
			transform.LookAt2d(Target.transform);
			transform.position = Vector2.MoveTowards(transform.position, Target.transform.position, Speed*Time.deltaTime);
		}
	}

	void findTarget() {
		Target = gameObject.CloestToObject(Game.Canons.ToArray());
		if (Target == null) Invoke("findTarget",1);
	}
	
}
