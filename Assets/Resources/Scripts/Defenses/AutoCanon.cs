using UnityEngine;
using System.Collections;

public class AutoCanon : Defense {
	GameObject target;
	GameObject bullet;

	protected  new void Start () {
		base.Start();

		InvokeRepeating("shoot",1,1);
		if (bullet == null) bullet = (Instantiate(Resources.Load("Prefabs/Bullet")) as GameObject);
	}
	
	protected new void Update () {
		base.Update();

		target = gameObject.CloestToObject(Game.Enemies.ToArray());
		if (target != null){
			transform.LookAt2d(target.transform,90);
		}
	}

	void shoot(){
		Instantiate (bullet, transform.position, transform.rotation);
	}

}
