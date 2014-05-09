using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// base class for enemy types. to create an enemy inherit this class
/// </summary>
public class Enemy : Liveable 
{
	public enum Type {spaceShip, Kamikazi}

	public bool IsMoving = false;
	public bool IsAutoMove = true;

	public float Speed = 10;
	public int Damage = 1;

	public GameObject Target;


	public void DecreaseHP(int amount)
	{
		Health -= amount;
	}

	void Start()
	{
		if(IsAutoMove == true)
			IsMoving = true;

		Game.Enemies.Add(this);
	}
	
	void onUpdate()
	{

	}
	
	void OnDestroy()
	{
		Game.Enemies.Remove(this);
	}



}
