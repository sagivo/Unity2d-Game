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
		Game.Enemies.Add(this);
	}

	void OnDestroy(){
		Game.Enemies.Remove(this);
	}
	
}
