using UnityEngine;
using System.Collections;

public class Kamikazi : Enemy {
	Hitable hit;
	float angle; 

	protected new void Awake(){
		base.Awake();
		damagePerLevel = new int[]{10,15,30};

		hit = GetComponent<Hitable>();
		hit.hits = new System.Type[]{ typeof(PlayerCanon), typeof(MineralMiner) };
		hit.damage = damagePerLevel[level];
	}

	protected new void Start(){
		base.Start();
		InvokeRepeating("findTarget",0,1);
	}
	 
	protected new void Update(){
		base.Update();

		if (target != null){
			transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed*Time.deltaTime);
		}
	}

	void findTarget() {
		if (null!=target) return;
		object[] targets = Game.mineralMiners.ToArray().Join(Game.player);
		target = gameObject.CloestToObject(targets);
		if (target && animator){
			angle = Extensions.AngelBetween(transform.position, target.transform.position);
			setValForAnimator("Direction", angle);
		}
	}
	
}
