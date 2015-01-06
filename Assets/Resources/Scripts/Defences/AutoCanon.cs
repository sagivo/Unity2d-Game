using UnityEngine;
using System.Collections;

public class AutoCanon : Building {
	public GameObject bullet;
	GameObject target;
	public float[] shootSpeedPerLevel = Vars.Balance.Player.AutoCanon.shootSpeedPerLevel;

	protected new void Awake(){
		base.Awake();
		buildCostPerLevel = Vars.Balance.Player.AutoCanon.buildCostPerLevel;
		buildTimePerLevel = Vars.Balance.Player.AutoCanon.upgradeTimePerLevel;
		refundPerLevel = Vars.Balance.Player.AutoCanon.refundPerLevel;
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

		if (status == StatusType.Live){
			target = gameObject.CloestToObject(Game.enemies.ToArray());
			if (target != null){
				transform.LookAt2d(target.transform,90);
			}
		}
	}

	void OnDestroy (){
		Game.autoCanons.Remove(this);
	}

	void shoot(){
		if (target != null) Instantiate (bullet, transform.position, transform.rotation);
	}

}
