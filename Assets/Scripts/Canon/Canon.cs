using UnityEngine;
using System.Collections;

public class Canon : Liveable
{

		//canon types
		public enum CanonType
		{
			Regular,
			AutoAim
		}
		
		//public
		public CanonType type;

		//bullet travel speed
		[Range(0.001f, 0.1f)] 
		public float bulletSpeed = 0.001f;


		//private 
		private bool isActive = false;
		
		//the ammo
		public GameObject ammoPrefab;
		
		//spawner for the bullets
		private GameObject bulletSpawnerLayer;

		private GameObject aquieredTarget;
	
		void Start ()
		{
			if (Game) {
					Game.Canons.Add (this);
			}
			
			//find the layer
			bulletSpawnerLayer = GameObject.FindGameObjectWithTag("SPAWNS");
		}
	
		void OnDestroy ()
		{
			if (Game) {
					Game.Canons.Remove (this);
			}
		}

		void Update ()
		{
			if(type == CanonType.Regular){
				//Rotating the canon to the croshair
				rotateToPosition(Input.mousePosition,this.transform.position);
				
				//handle mouse behaviours
				if (Input.GetMouseButtonDown (0)) {
					Fire ();
				}
			}
			else if(type == CanonType.AutoAim && aquieredTarget)
			{
				Debug.Log(aquieredTarget.transform.position.x);
				rotateToPosition(Camera.main.ScreenToWorldPoint(aquieredTarget.transform.position),this.transform.position);
			}
		}
		
		//fire 
		private void Fire ()
		{
			Vector3 pos = Input.mousePosition;
			pos.z = transform.position.z - Camera.main.transform.position.z;
			pos = Camera.main.ScreenToWorldPoint (pos);
	
			Quaternion q = Quaternion.FromToRotation (Vector3.up, pos - transform.position);
			GameObject go = (GameObject)Instantiate (ammoPrefab, this.transform.position, q);
			go.transform.parent = bulletSpawnerLayer.transform;
			go.rigidbody2D.AddForce (go.transform.up * bulletSpeed);
		}

		private void rotateToPosition(Vector3 mousePos,Vector3 originPos)
		{
			Vector3 canonWorldPos = Camera.main.WorldToScreenPoint (originPos);
			mousePos.x = mousePos.x - canonWorldPos.x;
			mousePos.y = mousePos.y - canonWorldPos.y;
			float angle = Mathf.Atan2 (mousePos.y, mousePos.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler (new Vector3 (0f, 0f, angle));
		}
	
		void OnTriggerEnter2D(Collider2D other) {
			if(type == CanonType.AutoAim && other.tag == "Enemy" && !aquieredTarget)
			{
				aquieredTarget = other.gameObject;
			}
		}
}
