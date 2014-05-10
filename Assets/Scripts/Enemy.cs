using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// base class for enemy types. to create an enemy inherit this class
/// </summary>
public class Enemy : Liveable {
	public enum Type {spaceShip, Kamikazi}
	public float Speed = 10;

	void Start(){
		if(Game)
		{
			Game.Enemies.Add(this);
		}
	}

	void OnDestroy(){
		if(Game)
		{
			Game.Enemies.Remove(this);
		}
	}

	void Update()
	{
		//used this for testing
//		transform.position = new Vector3(transform.position.x + 0.01f,transform.position.y,transform.position.z);
	}
}
