using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : Liveable 
{
	public float Speed = 3;
	public GameObject Target;

	//string[] hitTags = { "Bullet", "Player", "Enemy" };

	protected new void Start(){
		base.Start();
		Debug.Log("enemy");
		Game.Enemies.Add(this);

		OnHit = (o) => {
			Debug.Log("aaa");
			//if (System.Array.IndexOf(hitTags, o.gameObject.tag) > -1) Destroy(gameObject);
			if (o.gameObject.IsSubClassOf<Bullet>())
				Health -= o.gameObject.GetComponent<Bullet>().Damage;
		};

		OnDie += () => {
			Destroy(gameObject);
		};
	}

	void OnDestroy()
	{
		Game.Enemies.Remove(this);
	}
	

}
