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

	public void DecreaseHealth(int amount)
	{
		Health -= amount;
	}

	void Start()
	{
		Game.Enemies.Add(this);
		if(IsAutoMove == true) IsMoving = true;
	}
	
	void OnDestroy()
	{
		Game.Enemies.Remove(this);
	}



}
