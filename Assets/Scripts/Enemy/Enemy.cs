using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// base class for enemy types. to create an enemy inherit this class
/// </summary>
public class Enemy : Liveable 
{

	public enum Type {spaceShip, Kamikazi}

	public List<Enemy> Enemies = new List<Enemy>();

	protected bool _isMoving = false;
	public bool isMoving
	{
		get {return _isMoving;}
	}

	public bool isAutoMove = true;


	public float Speed = 10;

	public int HP = 1;

	public int Damage = 1;


	public System.Action OnMove;


	public GameObject Target;
	



	public void DecreaseHP(int amount)
	{
		HP -= amount;
	}



	public void StartMoving()
	{
		if(_isMoving == false)
		{
			_isMoving = true;
		}
	}
	
	public void StopMoving()
	{
		if(_isMoving == true)
		{
			_isMoving = false;
		}
	}

	void Start()
	{
		if(isAutoMove == true)
			StartMoving();

		if(Game == null)
			Game = GameController.Instance;
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
