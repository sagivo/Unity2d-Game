using UnityEngine;
using System.Collections;

public class CanonDestroyer : Enemy {
	public GameObject bullet;
	public float[] rangeShootPerLevel = new float[]{10, 20, 50};
	public float[] shootSpeedPerLevel = new float[]{1, 1.9f, 1.5f, 1};
	bool shooting;
	float angle;

	new void Awake(){
		base.Awake();
		damagePerLevel = new int[]{5,10,30};
		speed = .3f;
	}

	// Use this for initialization
	protected new void Start () {
		base.Start();
		//healthPerLevel = Vars.Balance.Enemy.canonDestroyer.healthPerLevel;
		//rangeShootPerLevel = Vars.Balance.Enemy.canonDestroyer.shootRangePerLevel;
		//shootSpeedPerLevel = Vars.Balance.Enemy.canonDestroyer.shootSpeedPerLevel;

		InvokeRepeating("findTarget",0,1);
	}
	
	// Update is called once per frame
	new void Update () {
		base.Update();
		if (target != null){
			if (Vector2.Distance(transform.position, target.transform.position) > rangeShootPerLevel[level]){
				//transform.LookAt2d(target.transform,(transform.position.x > target.transform.position.x) ? 180 :  0);
				transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed*Time.deltaTime);
			} else{ //shoot
				//transform.LookAt2d(target.transform,(transform.position.x > target.transform.position.x) ? 180 :  0);
				if (!shooting) {shooting = true; InvokeRepeating("shoot",0, shootSpeedPerLevel[level]);}
			}
		} else findTarget();
	}

	void findTarget() {
		if (null!=target) return;
		shooting = false;
		target = gameObject.CloestToObject(Game.autoCanons.ToArray());
		l (target);
		angle = Extensions.AngelBetween(transform.position, target.transform.position);
	}

	void shoot(){
		if (target != null ) {
			angle = Extensions.AngelBetween(transform.position, target.transform.position);
			l (angle);
			animator.SetFloat("Direction", angle);
			var b = Instantiate (bullet, transform.position, Quaternion.Euler(0, 0, angle)) as GameObject;
			//b.transform.Rotate(0,0,90);
			var h = b.GetComponent<Hitable>();
			h.hits = new System.Type[]{ typeof(AutoCanon) };
			h.damage = damagePerLevel[level];
		}
	}
}
