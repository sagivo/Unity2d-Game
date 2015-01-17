using UnityEngine;
using System.Collections;

public class Orc : Enemy {
	public float[] attackSpeedPerLevel = new float[]{0, 1.9f, 1.5f, 1};
	Hitable hit;
	float distanceToHit = 3;
	bool attacking;
	
	protected new void Awake(){
		base.Awake();
		damagePerLevel = new int[]{20,15,30};
		
		hit = GetComponent<Hitable>();
		hit.hits = new System.Type[]{ typeof(PlayerCanon), typeof(MineralMiner), typeof(AutoCanon) };
		l (hit.hits.Length);
		hit.damage = damagePerLevel[level];
	}
	
	protected new void Start(){
		base.Start();
		InvokeRepeating("findTarget",0,1);
	}
	
	protected new void Update(){
		base.Update();
		
		if (target != null){
			if (Vector2.Distance(transform.position, target.transform.position) > distanceToHit){
				if (target.transform.position.x > transform.position.x && !lookRight) Flip();
				else if(target.transform.position.x < transform.position.x && lookRight) Flip();
				
				var prevPos = transform.position;
				transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed*Time.deltaTime);
				animator.SetFloat("speed", (transform.position - prevPos).magnitude/Time.deltaTime);
			} else{ //shoot
				if (!attacking) {attacking = true; CancelInvoke("attack"); InvokeRepeating("attack",attackSpeedPerLevel[level], attackSpeedPerLevel[level]); animator.SetFloat("speed",0);}
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
			animator.SetTrigger("attack");
		}
	}
	
}