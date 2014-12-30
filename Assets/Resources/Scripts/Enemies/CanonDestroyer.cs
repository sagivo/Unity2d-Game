using UnityEngine;
using System.Collections;

public class CanonDestroyer : Enemy {

	// Use this for initialization
	new void Start () {
		base.Start();
		speed = .7f;

		findTarget();
	}
	
	// Update is called once per frame
	new void Update () {
		if (target != null){
			transform.LookAt2d(target.transform,180);
			transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed*Time.deltaTime);
		} else Invoke("findTarget",1);
	}

	void findTarget() {
		l (Game.autoCanons.ToArray().Length);
		target = gameObject.CloestToObject(Game.autoCanons.ToArray());
		if (target == null) Invoke("findTarget",1);
	}
}
