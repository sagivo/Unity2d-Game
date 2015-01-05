using UnityEngine;
using System.Collections;

public class Kamikazi : Enemy {
	public int damage = 20;

	protected new void Awake(){
		base.Awake();
		speed = 1.1f;
		health = 40;
	}

	protected new void Start(){
		base.Start();
		l ("kami");
		target = Game.player;
	}
	 
	protected new void Update(){
		base.Update();

		if (target != null){
			transform.LookAt2d(target.transform);
			transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed*Time.deltaTime);
		} else Invoke("findTarget",1);
	}

	void findTarget() {
		target = Game.player;
		if (target == null) Invoke("findTarget",1);
	}
	
}
