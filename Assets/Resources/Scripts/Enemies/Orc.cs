using UnityEngine;
using System.Collections;

public class Orc : Enemy {
	Hitable hit;
	float angle; 
	
	protected new void Awake(){
		base.Awake();
		damagePerLevel = new int[]{20,15,30};
		
		hit = GetComponent<Hitable>();
		hit.hits = new System.Type[]{ typeof(PlayerCanon), typeof(MineralMiner), typeof(AutoCanon) };
		hit.damage = damagePerLevel[level];
	}
	
	protected new void Start(){
		base.Start();
		InvokeRepeating("findTarget",0,1);
	}
	
	protected new void Update(){
		base.Update();
		
		if (target != null){
			if (target.transform.position.x > transform.position.x && !lookRight) Flip();
			else if(target.transform.position.x < transform.position.x && lookRight) Flip();

			var prevPos = transform.position;
			transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed*Time.deltaTime);
			animator.SetFloat("speed", (transform.position - prevPos).magnitude/Time.deltaTime);
		}
	}
	
	void findTarget() {
		if (null!=target) return;
		object[] targets = Game.mineralMiners.ToArray().Join(Game.player);
		target = gameObject.CloestToObject(targets);
		l (target);
		if (target && animator){
			l ("find");
			angle = Extensions.AngelBetween(transform.position, target.transform.position);
			setValForAnimator("Direction", angle);
		}
	}
	
}