using UnityEngine;
using System.Collections;

public class AutoCanon : Building {
	public GameObject bullet;
	GameObject target;
	[System.NonSerialized]
	float[] shootSpeedPerLevel;


	protected new void Awake(){
		base.Awake();
	}

	protected  new void Start () {
		base.Start();
		//healthPerLevel = Vars.Balance.Player.AutoCanon.healthPerLevel;
		//buildTimePerLevel = Vars.Balance.Player.AutoCanon.upgradeTimePerLevel;
		//refundPerLevel = Vars.Balance.Player.AutoCanon.refundPerLevel;
		//shootSpeedPerLevel = Vars.Balance.Player.AutoCanon.shootSpeedPerLevel;
		//buildCostPerLevel =  Vars.Balance.Player.AutoCanon.buildCostPerLevel;

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
		if (target != null) {
			var b = Instantiate (bullet, transform.position, transform.rotation) as GameObject;
			b.GetComponent<Bullet>().hits = new System.Type[]{ typeof(Kamikazi), typeof(CanonDestroyer)};
		}
	}
	
}
