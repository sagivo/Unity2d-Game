using UnityEngine;
using System.Collections;

public class Kamikazi : Enemy {
	public int damage = 30;

	protected new void Start(){
		base.Start();
		findTarget();
	}
	 
	protected new void Update(){
		base.Update();

		if (target != null){
			transform.LookAt2d(target.transform);
			transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed*Time.deltaTime);
		}
	}

	void findTarget() {
		target = gameObject.CloestToObject(Game.Canons.ToArray());
		if (target == null) Invoke("findTarget",1);
	}
	
}
