using UnityEngine;
using System.Collections;

public class Liveable : BaseObj {

	public delegate void OnHitDel(Collider2D o);
	public OnHitDel OnHit;

	public GameObject healthBar;

	public int Health {set{ health += value; if (OnHealthChanged!=null) OnHealthChanged(Health); if (health <= 0 && OnDie!=null) OnDie(); } get{return health;}}
	public int Level;
	public enum StatusType {Live, Destroyed, Upgrading, Repair}
	public StatusType Status;

	//public System.Action<Collider2D> OnHit;
	public System.Action OnDie;
	public System.Action<object> OnBuildStart;
	public System.Action<object> OnBuildEnd;
	public System.Action<object> OnRepairStart;
	public System.Action<object> OnRepairEnd;
	public System.Action<object> OnHealthChanged;

	int health = 100;

	protected new void Start(){
		base.Start();
		Debug.Log("live");
		Status = StatusType.Live;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log((OnHit != null));
		if (OnHit != null) OnHit(other);
	}

}
