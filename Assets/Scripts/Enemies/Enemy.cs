using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : Liveable 
{
	public float Speed = 10;
	public GameObject Target;

	void Start() 
	{
		Game.Enemies.Add(this);
	}

	void OnDestroy()
	{
		Game.Enemies.Remove(this);
	}

}
