using UnityEngine;
using System.Collections;

public class AutoCanon : Building {
	public Bullet bullet;
	GameObject target;
	bool shooting;
	[Header("Canon Confid")]
	public int[] damagePerLevel = new int[]{0, 10, 10, 10, 10, 40, 100};
	public float[] shootSpeedPerLevel;

	protected new void Awake(){
		base.Awake();

		Game.autoCanons.Add(this);
		OnStatusChange += (s)=>{
			CancelInvoke("shoot");
			if (status == StatusType.Live) InvokeRepeating("shoot", shootSpeedPerLevel[level], shootSpeedPerLevel[level]);
		};
		
		OnDie += () => {
			Game.autoCanons.Remove(this);
		};
	}

	protected  new void Start () {
		base.Start();

		InvokeRepeating("findTarget",0,1);
	}
	
	protected new void Update () {
		base.Update();
		if (status == StatusType.Live && null != target){
			//transform.LookAt2d(target.transform, 90);
			if (!shooting) {shooting = true; CancelInvoke("shoot"); InvokeRepeating("shoot",shootSpeedPerLevel[level], shootSpeedPerLevel[level]);}
		}
	}

	void shoot(){
		if (target != null) {
			var angle = Extensions.AngelBetween(transform.position, target.transform.position);

			var h = (Instantiate (bullet, transform.position, Quaternion.Euler(0, 0, angle)) as Bullet).GetComponent<Hitable>();
			h.hits = new System.Type[]{ typeof(Kamikazi), typeof(CanonDestroyer)};
			h.damage = damagePerLevel[level];

			setValForAnimator("Direction", angle);
		}
	}

	void findTarget() {
		if (null!=target) return;
		shooting = false;
		target = gameObject.CloestToObject(Game.enemies.ToArray());
	}

}
