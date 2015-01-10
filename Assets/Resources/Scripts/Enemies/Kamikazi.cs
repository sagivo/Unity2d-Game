using UnityEngine;
using System.Collections;

public class Kamikazi : Enemy {
	Hitable hit;

	protected new void Awake(){
		base.Awake();
		speed = 1.1f;
		damagePerLevel = new int[]{10,15,30};

		hit = GetComponent<Hitable>();
		hit.hits = new System.Type[]{ typeof(PlayerCanon), typeof(MineralMiner) };
		hit.damage = damagePerLevel[level];
	}

	protected new void Start(){
		base.Start();
		findTarget();
		//healthPerLevel = Vars.Balance.Enemy.kamikazi.healthPerLevel;
	}
	 
	protected new void Update(){
		base.Update();

		if (target != null){
			transform.LookAt2d(target.transform);
			transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed*Time.deltaTime);
		} else Invoke("findTarget",1);
	}

	void findTarget() {
		//target = Game.player;

		object[] targets = Game.mineralMiners.ToArray().Join(Game.player);
		target = gameObject.CloestToObject(targets);
		if (target == null) Invoke("findTarget",1);
	}
	
}
