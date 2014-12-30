using UnityEngine;
using System.Collections;

public class Canon : Building
{
	//canon types
	public enum CanonType{Regular, AutoAim}

	public CanonType type;
	[Range(0.001f, 0.1f)] 
	public float bulletSpeed = 0.001f;
	[Range(5, 0.1f)] 
	public float fireRate = 1f;
	public Bullet ammoPrefab;
	public int[] damageExtraPerLevel = new int[]{0, 10, 10, 10, 10, 40, 100};


	//the current target for the auto aim
	GameObject target;
	float nextShoot;

	protected new void Start(){
		base.Start();
		Game.canons.Add (this);
		
		nextShoot += fireRate;
		OnHit += (o) => {
			if (o.gameObject.IsSubClassOf<Kamikazi>()){						
				DecHealth(o.gameObject.GetComponent<Kamikazi>().damage);
			}
		};

		OnDie += () => {
			Destroy(gameObject);
		};
	}

	void OnDestroy (){
		Game.canons.Remove(this);
	}

	protected new void Update(){
		base.Update();
		if (type == CanonType.Regular) {
			rotateToPosition (Input.mousePosition, this.transform.position);
			if (Input.GetMouseButtonDown (0))
				Fire (Input.mousePosition); //TODO: change to touch
		} else if (type == CanonType.AutoAim && target) {
			rotateToPosition (Camera.main.WorldToScreenPoint (target.transform.position), this.transform.position);				
			if (nextShoot < Time.time) {
				Fire (Camera.main.WorldToScreenPoint (target.transform.position));
				nextShoot = Time.time + fireRate;
			}
		}
	}

	//fire 
	private void Fire (Vector3 pos)
	{
		pos.z = transform.position.z - Camera.main.transform.position.z;
		pos = Camera.main.ScreenToWorldPoint (pos);
		Quaternion q = Quaternion.FromToRotation (Vector3.up, pos - transform.position);
		Bullet b = (Bullet)Instantiate (ammoPrefab, this.transform.position, q);
		b.damage += damageExtraPerLevel[level];
		b.transform.parent = Game.spawnerLayer.transform;
		b.rigidbody2D.AddForce (b.transform.up * bulletSpeed);
	}

	private void rotateToPosition(Vector3 mousePos, Vector3 originPos)
	{
			Vector3 canonWorldPos = Camera.main.WorldToScreenPoint (originPos);
			mousePos.x = mousePos.x - canonWorldPos.x;
			mousePos.y = mousePos.y - canonWorldPos.y;
			float angle = Mathf.Atan2 (mousePos.y, mousePos.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler (new Vector3 (0f, 0f, angle - 90));
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (type == CanonType.AutoAim && other.tag == "Enemy" && !target) {
			target = other.gameObject;
		}
		if (OnHit != null) OnHit(other);
	}
}
