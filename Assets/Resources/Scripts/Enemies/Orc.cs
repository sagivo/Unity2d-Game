using UnityEngine;
using System.Collections;

public class Orc : Enemy {
	public float[] attackSpeedPerLevel = new float[]{0, 1.9f, 1.5f, 1};
	Hitable hit;
	float distanceToHit = 3;
	bool attacking;
	Transform axe;

	protected new void Awake(){
		base.Awake();
		damagePerLevel = new int[]{20,15,30};

		OnUpgraded += () => {
			spriteRenderer = transform.SearchByName("orc_head").GetComponent<SpriteRenderer>();
			axe = transform.SearchByName("orc_weapon");
		};
	}
	
	protected new void Start(){
		base.Start();
		InvokeRepeating("findTarget",0,1);
	}
	
	protected new void Update(){
		base.Update();
		
		if (target != null){
			if (Vector2.Distance(axe.position, target.transform.position) > distanceToHit){
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
			if (null==hit){
				hit = GetComponentInChildren<Hitable>();
				hit.keepAlive = true;
				hit.hits = new System.Type[]{ typeof(PlayerCanon), typeof(MineralMiner), typeof(AutoCanon) };
				hit.damage = damagePerLevel[level];
			}
			hit.enabled = true;
			animator.SetTrigger("attack");
		} else {
			attacking = false;
			CancelInvoke("attack");
			hit.enabled = false;
		}
	}
	
}