using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
	public static GameController Instance; //always call this instance to get the current game
	public int Level;
	public GameObject Player;
	public List<Enemy> Enemies = new List<Enemy>(); //populate this list each time you create an enemy.

	void Awake(){
		Instance = this;
	}

}
