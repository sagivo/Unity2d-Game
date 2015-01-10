using UnityEngine;
using System.Collections;

public class AutoCanon : Building {
	public Bullet bullet;
	GameObject target;
	float[] shootSpeedPerLevel = new float[]{1,.5f,.2f};
	int[] damagePerLevel = new int[]{0, 10, 10, 10, 10, 40, 100};
	bool shooting;

	protected new void Awake(){
		base.Awake();

		OnStatusChange += (s)=>{
			CancelInvoke("shoot");
			if (status == StatusType.Live) InvokeRepeating("shoot",shootSpeedPerLevel[level], shootSpeedPerLevel[level]);
		};
		
		OnDie += () => {
			Game.autoCanons.Remove(this);
		};
	}

	protected  new void Start () {
		base.Start();
		Game.autoCanons.Add(this);

		InvokeRepeating("findTarget",0,1);
	}
	
	protected new void Update () {
		base.Update();
		if (status == StatusType.Live && null != target){
			transform.LookAt2d(target.transform,90);
			if (!shooting) {shooting = true; InvokeRepeating("shoot",0, shootSpeedPerLevel[level]);}
		}
	}

	void shoot(){
		if (target != null) {
			shooting = true;
			var h = (Instantiate (bullet, transform.position, transform.rotation) as Bullet).GetComponent<Hitable>();
			h.hits = new System.Type[]{ typeof(Kamikazi), typeof(CanonDestroyer)};
			h.damage = damagePerLevel[level];
		}
	}

	void findTarget() {
		if (null!=target) return;
		shooting = false;
		target = gameObject.CloestToObject(Game.enemies.ToArray());
	}

}
