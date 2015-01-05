using UnityEngine;
using System.Collections;

public class HealthBarController : BaseObj
{	
	float health = 100f;	

	GameObject hbProgress;
	public float yMargin = 2;

	protected new void Start (){
		if (transform.Find("HP Bar")) hbProgress = transform.Find("HP Bar").gameObject;
	}

	protected new void Update ()
	{
		transform.rotation = Quaternion.identity;
		if (transform.parent) 
			transform.position = new Vector2( transform.parent.transform.position.x , transform.parent.transform.position.y - yMargin); 
	}

	public void setHealth(float newHealth){
		health=newHealth;

		if (hbProgress == null) return;
		float originalValue = hbProgress.renderer.bounds.min.x;
		float calculate = Mathf.Clamp (health / 100f, 0f, 1f);
		hbProgress.transform.localScale = new Vector3 (calculate, 1f, 1f);
		float newValue = hbProgress.renderer.bounds.min.x;
		float difference = newValue - originalValue;
		hbProgress.transform.Translate(new Vector2 (-difference, 0));
	}
}