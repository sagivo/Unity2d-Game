using UnityEngine;
using System.Collections;

public class HealthBarController : BaseObj
{	
	float health = 100;
	[System.NonSerialized]
	public float divider;

	GameObject hbProgress;
	public float yMargin = 2;

	protected new void Start (){
		base.Start();

		if (transform.Find("HP Bar")) hbProgress = transform.Find("HP Bar").gameObject;
	}

	protected new void Update ()
	{
		transform.rotation = Quaternion.identity;
		if (transform.parent) 
			transform.position = new Vector2( transform.parent.transform.position.x , transform.parent.transform.position.y - yMargin); 
	}
	//int count=0;

	public void setHealth(float newHealth){
		//while (newHealth==0 && count < 10000) {count++; yield return null;}
		health = newHealth;

		if (hbProgress != null){
			float originalValue = hbProgress.renderer.bounds.min.x;
			float calculate = Mathf.Clamp (health / divider, 0f, 1f);
			hbProgress.transform.localScale = new Vector3 (calculate, 1f, 1f);
			float newValue = hbProgress.renderer.bounds.min.x;
			float difference = newValue - originalValue;
			hbProgress.transform.Translate(new Vector2 (-difference, 0));
		}
	}
}