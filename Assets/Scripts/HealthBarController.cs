using UnityEngine;
using System.Collections;

public class HealthBarController : MonoBehaviour {
	
	public float health = 1.0f;	
	public GameObject hbProgress;

	private float originalHealth = 1.0f;
	
	// Update is called once per frame
	void Update () {
		float originalValue = hbProgress.renderer.bounds.min.x;
		float ration = health / originalHealth;
		hbProgress.transform.localScale = new Vector3(ration,1f,1f);
		float newValue = hbProgress.renderer.bounds.min.x;
		float difference = newValue - originalValue;
		hbProgress.transform.Translate(new Vector3(-difference, 0f, 0f));

		transform.rotation = new Quaternion(0f,0f,0f,-transform.parent.rotation.z);
	}
}
