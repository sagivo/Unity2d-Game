using UnityEngine;
using System.Collections;

public class Liveable : BaseObj {

	public GameObject healthBar;

	public int Health {set{ health += value; if (OnHealthChanged!=null) OnHealthChanged(Health); if (health <= 0 && OnDie!=null) OnDie(); } get{return health;}}
	public int Level;
	public enum StatusType {Live, Destroyed, Upgrading, Repair}
	public StatusType Status;

	public System.Action<object> OnHit;
	public System.Action OnDie;
	public System.Action<object> OnBuildStart;
	public System.Action<object> OnBuildEnd;
	public System.Action<object> OnRepairStart;
	public System.Action<object> OnRepairEnd;
	public System.Action<object> OnHealthChanged;

	int health = 100;

	void Start(){
		Status = StatusType.Live;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (OnHit != null) OnHit(other);
	}

}
