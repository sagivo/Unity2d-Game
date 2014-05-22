using UnityEngine;
using System.Collections;

public class HealthBarController : BaseObj
{	
	public float health = 100f;	

	GameObject hbProgress;
	protected new void Start (){
		hbProgress = transform.Find("HP Bar").gameObject;
	}

	protected new void Update ()
	{
		if (hbProgress == null) return;
		float originalValue = hbProgress.renderer.bounds.min.x;
		float calculate = Mathf.Clamp (health / 100f, 0f, 1f);
		hbProgress.transform.localScale = new Vector3 (calculate, 1f, 1f);
		float newValue = hbProgress.renderer.bounds.min.x;
		float difference = newValue - originalValue;
		hbProgress.transform.Translate (new Vector3 (-difference, 0f, 0f));
		if (transform.parent) transform.rotation = new Quaternion (0f, 0f, 0f, -transform.parent.rotation.z);	

		//TODO: renderer.bounds.extents to set height 
	}
}