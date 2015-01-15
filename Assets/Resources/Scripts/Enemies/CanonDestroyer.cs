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
	}

	protected new void Start () {
		base.Start();
		InvokeRepeating("findTarget",1,1);
		spriteRenderer.color = Color.magenta;
	}
	
	new void Update () {
		base.Update();
		if (target != null){
			if (Vector2.Distance(transform.position, target.transform.position) > rangeShootPerLevel[level]){
				transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed*Time.deltaTime);
			} else{ //shoot
				if (!shooting) {shooting = true; CancelInvoke("shoot"); InvokeRepeating("shoot",shootSpeedPerLevel[level], shootSpeedPerLevel[level]);}
			}
		} 
	}

	void findTarget() {
		if (null!=target) return;
		shooting = false;
		target = gameObject.CloestToObject(Game.autoCanons.ToArray());
		if (target && animator){
			angle = Extensions.AngelBetween(transform.position, target.transform.position);
			setValForAnimator("Direction", angle);
		}
	}

	void shoot(){
		if (target != null ) {
			var b = Instantiate (bullet, transform.position, Quaternion.Euler(0, 0, angle)) as GameObject;
			var h = b.GetComponent<Hitable>();
			h.hits = new System.Type[]{ typeof(AutoCanon) };
			h.damage = damagePerLevel[level];
		}
	}
}
