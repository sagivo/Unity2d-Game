using UnityEngine;
using System.Collections;

public class Liveable : BaseObj {

	public GameObject healthBar;

	public int Health;
	public int Level;
	public enum Status {Live, Destroyed, Upgrading, Repair}

	public System.Action<object> OnHit;
	public System.Action<object> OnDie;
	public System.Action<object> OnBuildStart;
	public System.Action<object> OnBuildEnd;
	public System.Action<object> OnRepairStart;
	public System.Action<object> OnRepairEnd;



}
