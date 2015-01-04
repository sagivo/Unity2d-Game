using UnityEngine;
using System.Collections;

public class AutoCanon : Building {
	public GameObject bullet;
	GameObject target;
	public float[] shootSpeedPerLevel = new float[]{1, .7f, .3f, .2f};

	protected new void Awake(){
		base.Awake();
		buildTime = 2f;
		mineralCostUpgrade = new int[]{5,10,20,40,80};
		upgradeTimePerLevel = new float[]{2,5,10,20};
	}

	protected  new void Start () {
		base.Start();
		Game.autoCanons.Add(this);

		OnStatusChange += (s)=>{
			if (status == StatusType.Live){
				CancelInvoke("shoot");
				InvokeRepeating("shoot",shootSpeedPerLevel[level], shootSpeedPerLevel[level]);
			}
		};
	}
	
	protected new void Update () {
		base.Update();

		target = gameObject.CloestToObject(Game.enemies.ToArray());
		if (target != null){
			transform.LookAt2d(target.transform,90);
		}
	}

	void OnDestroy (){
		Game.autoCanons.Remove(this);
	}

	void shoot(){
		if (target != null) Instantiate (bullet, transform.position, transform.rotation);
	}

}
