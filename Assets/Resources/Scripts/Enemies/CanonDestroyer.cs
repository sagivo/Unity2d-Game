using UnityEngine;
using System.Collections;

public class CanonDestroyer : Enemy {
	public GameObject bullet;
	protected override int[] healthPerLevel{get{return Vars.Balance.Enemy.canonDestroyer.healthPerLevel;}}
	float[] rangeShootPerLevel = Vars.Balance.Enemy.canonDestroyer.shootRangePerLevel;
	float[] shootSpeedPerLevel = Vars.Balance.Enemy.canonDestroyer.shootSpeedPerLevel;

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
			if (Vector2.Distance(transform.position, target.transform.position) > rangeShootPerLevel[level]){
				transform.LookAt2d(target.transform,(transform.position.x > target.transform.position.x) ? 180 :  0);
				transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed*Time.deltaTime);
			} else{ //shoot
				InvokeRepeating("shoot",shootSpeedPerLevel[level], shootSpeedPerLevel[level]);
			}
		} else Invoke("findTarget",1);
	}

	void findTarget() {
		target = gameObject.CloestToObject(Game.autoCanons.ToArray());
		if (target == null) Invoke("findTarget",1);
	}

	void shoot(){
		if (target != null ) {
			var b = Instantiate (bullet, transform.position, transform.rotation) as GameObject;
			b.GetComponent<Bullet>().hits = new System.Type[]{ typeof(AutoCanon)};
		}
	}
}
