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

			transform.LookAt(Target.transform.position, Vector3.forward);

			//Vector3 dir = Target.transform.position - transform.position;
			//float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
			//transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		}
	}

	void findTarget() {
		Target = Helpers.CloestToObject(Game.Canons.ToArray(), gameObject);
		if (Target == null) Invoke("findTarget", 2);
	}
}
