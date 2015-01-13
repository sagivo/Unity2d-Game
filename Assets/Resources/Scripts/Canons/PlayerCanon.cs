using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerCanon : Building
{
	//canon types
	//public enum CanonType{Regular, AutoAim}

	//public CanonType type;
	[Range(0.001f, 0.1f)] 
	public float bulletSpeed = 0.001f;
	[Range(5, 0.1f)] 
	public float fireRate = 1f;
	public Bullet bullet;
	int[] damagePerLevel = new int[]{5, 10, 10, 10, 10, 40, 100};

	//the current target for the auto aim
	GameObject target;
	float nextShoot;

	protected new void Awake(){
		base.Awake();
	}

	protected new void Start(){
		base.Start();
		//healthPerLevel = Vars.Balance.Player.PlayerCanon.healthPerLevel;
		Game.player = this;

		nextShoot += fireRate;

		OnDie += () => {
			Game.player = null;
		};
	}

	protected new void Update(){
		base.Update();
		rotateToPosition (Input.mousePosition, this.transform.position);
		if (Input.GetMouseButtonDown (0))
			Fire (Input.mousePosition); //TODO: change to touch		
	}

	//fire 
	private void Fire (Vector3 pos)
	{
		pos.z = transform.position.z - Camera.main.transform.position.z;
		pos = Camera.main.ScreenToWorldPoint (pos);
		//Quaternion q = Quaternion.FromToRotation (Vector3.up, pos - transform.position);

		var h = (Instantiate (bullet, transform.position, transform.rotation ) as Bullet).GetComponent<Hitable>();
		h.transform.Rotate(new Vector3(0,0,270));
		h.hits = new System.Type[]{ typeof(Kamikazi), typeof(CanonDestroyer)};
		h.damage = damagePerLevel[level];
	}

	private void rotateToPosition(Vector3 mousePos, Vector3 originPos)
	{
		var angle = Extensions.AngelBetween(originPos, mousePos);
		setValForAnimator("Direction", angle);
	}

}
