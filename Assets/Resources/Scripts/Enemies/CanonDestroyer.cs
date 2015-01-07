using UnityEngine;
using System.Collections;

public class CanonDestroyer : Enemy {
	protected override int[] healthPerLevel{get{return Vars.Balance.Enemy.canonDestroyer.healthPerLevel;}}

	new void Awake(){
		base.Awake();
		speed = .3f;
	}

	// Use this for initialization
	protected new void Start () {
		base.Start();
		findTarget();
	}
	
	// Update is called once per frame
	new void Update () {
		base.Update();
		if (target != null){
			transform.LookAt2d(target.transform,(transform.position.x > target.transform.position.x) ? 180 :  0);
			transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed*Time.deltaTime);
		} else Invoke("findTarget",1);
	}

	void findTarget() {
		target = gameObject.CloestToObject(Game.autoCanons.ToArray());
		if (target == null) Invoke("findTarget",1);
	}
}
