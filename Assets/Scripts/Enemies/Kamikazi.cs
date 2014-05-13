using UnityEngine;
using System.Collections;

public class Kamikazi : Enemy {

	void Start () {
		findTarget();
	}

	void Update () {
		if (Target != null){
			//transform.LookAt(Target.transform);
			transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, Speed*Time.deltaTime);
		}
	}

	void findTarget() {
		Target = Helpers.CloestToObject(Game.Canons.ToArray(), gameObject);
		if (Target == null) Invoke("findTarget", 2);
	}
}
