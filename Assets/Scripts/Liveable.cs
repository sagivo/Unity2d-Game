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

	protected void LookAt2d(Transform target){
		var dir = Camera.main.ScreenToWorldPoint(target.position) - Camera.main.ScreenToWorldPoint(transform.position);
		var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg -90;
		transform.rotation = Quaternion.Euler (new Vector3 (0f, 0f, angle));
	}

}
