using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : Liveable 
{
	public float Speed = 3;
	public GameObject Target;

	string[] hitTags = { "Bullet", "Player", "Enemy" };
	void Start() 
	{
		Game.Enemies.Add(this);
		OnHit += (o) => {
			if (System.Array.IndexOf(hitTags, (o as Collider2D).gameObject.tag) > -1) Destroy(gameObject);
		};
	}

	void OnDestroy()
	{
		Game.Enemies.Remove(this);
	}
	

}
