using UnityEngine;
using System.Collections;

public class AutoCanon : Defence {
	public GameObject bullet;
	GameObject target;

	protected  new void Start () {
		base.Start();

		//if (bullet == null) bullet = (Instantiate(Resources.Load("Prefabs/Bullet")) as GameObject);
		InvokeRepeating("shoot",1,1);
	}
	
	protected new void Update () {
		base.Update();

		target = gameObject.CloestToObject(Game.Enemies.ToArray());
		if (target != null){
			transform.LookAt2d(target.transform,90);
		}
	}

	void shoot(){
		if (target != null) Instantiate (bullet, transform.position, transform.rotation);
	}

}
