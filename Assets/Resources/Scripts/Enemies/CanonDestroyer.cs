using UnityEngine;
using System.Collections;

public class CanonDestroyer : Enemy {
	public GameObject bullet;
	float[] rangeShootPerLevel = new float[]{10, 20, 50};
	float[] shootSpeedPerLevel = new float[]{1, 1.9f, 1.5f, 1};
	bool shooting;

	new void Awake(){
		base.Awake();
		speed = .3f;
	}

	// Use this for initialization
	protected new void Start () {
		base.Start();
		//healthPerLevel = Vars.Balance.Enemy.canonDestroyer.healthPerLevel;
		//rangeShootPerLevel = Vars.Balance.Enemy.canonDestroyer.shootRangePerLevel;
		//shootSpeedPerLevel = Vars.Balance.Enemy.canonDestroyer.shootSpeedPerLevel;

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
				transform.LookAt2d(target.transform,(transform.position.x > target.transform.position.x) ? 180 :  0);
				if (!shooting) {shooting = true; InvokeRepeating("shoot",0, shootSpeedPerLevel[level]);}
			}
		} else findTarget();
	}

	void findTarget() {
		CancelInvoke("findTarget"); shooting = false;
		target = gameObject.CloestToObject(Game.autoCanons.ToArray());
		if (target == null) Invoke("findTarget",1);
	}

	void shoot(){
		if (target != null ) {
			var b = Instantiate (bullet, transform.position, transform.rotation) as GameObject;
			b.transform.Rotate(0,0,90);
			b.GetComponent<Bullet>().hits = new System.Type[]{ typeof(AutoCanon) };
		}
	}
}
