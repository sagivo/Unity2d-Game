using UnityEngine;
using System.Collections;

public class AutoCanon : Defense {
	GameObject target;

	protected  new void Start () {
		base.Start();

	}
	
	protected new void Update () {
		base.Update();

		target = gameObject.CloestToObject(Game.Enemies.ToArray());
		if (target != null){
			L(target.transform.position.x);
			transform.LookAt2d(target.transform,90);
		}
	}

}
