using UnityEngine;
using System.Collections;

public class Goblin : Enemy {
	public GameObject bullet;
	public float[] rangeShootPerLevel = new float[]{10, 20, 50};
	public float[] attackSpeedPerLevel = new float[]{0, 1.9f, 1.5f, 1};
	bool attacking;
	float angle;
	
	protected new void Awake(){
		base.Awake();
		
		OnUpgraded += () => {
			spriteRenderer = transform.SearchByName("goblin_head").GetComponent<SpriteRenderer>();
		};
		OnHit+= (o) => {
			l (o);
		};
	}
	
	protected new void Start(){
		base.Start();
		InvokeRepeating("findTarget",0,1);
	}
	
	protected new void Update(){
		base.Update();
		
		if (target != null){
			if (Vector2.Distance(transform.position, target.transform.position) > rangeShootPerLevel[level]){
				if (target.transform.position.x > transform.position.x && !lookRight) Flip();
				else if(target.transform.position.x < transform.position.x && lookRight) Flip();
				
				transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed*Time.deltaTime);
				setValForAnimator("speed", speed);
			} else{ //shoot
				setValForAnimator("speed", 0);
				if (!attacking) {attacking = true; CancelInvoke("attack"); InvokeRepeating("attack",attackSpeedPerLevel[level], attackSpeedPerLevel[level]); }
			}
		}
	}
	
	void findTarget() {
		if (null!=target) return;
		object[] targets = Game.mineralMiners.ToArray().Join(Game.player).Join (Game.autoCanons.ToArray());
		target = gameObject.CloestToObject(targets);
	}
	
	void attack(){
		if (target != null ) {
			angle = Extensions.AngelBetween(transform.position, target.transform.position);
			var b = Instantiate (bullet, transform.position, Quaternion.Euler(0, 0, angle)) as GameObject;
			var h = b.GetComponent<Hitable>();
			h.hits = new System.Type[]{ typeof(AutoCanon) };
			h.damage = damagePerLevel[level];

			animator.SetTrigger("attack");
		} else {
			attacking = false;
			CancelInvoke("attack");
		}
	}
	
}
