using UnityEngine;
using System.Collections;

public class Liveable : BaseObj {

	public int Health {get{return health;} }
	public int level;
	public enum StatusType {Live, Destroyed, Upgrading, Repair}
	public StatusType status;
	public bool showHealthBar = true;

	public System.Action<Collider2D> OnHit;
	public System.Action OnDie;
	public System.Action<object> OnBuildStart;
	public System.Action<object> OnBuildEnd;
	public System.Action<object> OnRepairStart;
	public System.Action<object> OnRepairEnd;
	public System.Action<object> OnHealthChanged;

	int health = 100;
	HealthBarController healthBar;

	public int mineralCost = 10;
	public int mineralCostRefund = 6;
	public int diamondCost = 1;

	protected new void Start(){
		base.Start();

		if (Game.minerals >= mineralCost) Game.minerals-= mineralCost; 
		else Game.diamonds -= diamondCost;

		status = StatusType.Live;
		if (showHealthBar) {

			var hb = Instantiate(Resources.Load("Prefabs/HealthBar"),new Vector3(transform.position.x, transform.position.y - transform.lossyScale.y),Quaternion.identity) as GameObject;
			hb.transform.parent = transform;
			healthBar = hb.AddComponent<HealthBarController>(); healthBar.health = Health;
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (OnHit != null) OnHit(other);
	}

	public void DecHealth(int by){
		health -= by;
		if (OnHealthChanged!=null) OnHealthChanged(Health); 
		if (health <= 0 && OnDie!=null) OnDie();
		healthBar.health = Health;
	}

	public bool enoughResourcesToCreate(){
		return (Game.minerals >= mineralCost || Game.diamonds >= diamondCost);
	}

	public void refund(){
		Game.minerals += mineralCostRefund;
	}
}
