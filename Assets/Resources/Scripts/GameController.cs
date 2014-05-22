using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : Singleton<GameController> {
	public int Level;
	public Player Player;
	public int Score;
	public List<Resource> Resources = new List<Resource>();
	public List<Cell> Cells = new List<Cell>();
	public List<Enemy> Enemies = new List<Enemy>(); //populate this list each time you create an enemy.
	public List<Building> Buildings = new List<Building>(); //populate this list each time you create a building.
	public List<Canon> Canons = new List<Canon>(); //populate this list each time you create a building.
	public List<Defense> Defenses = new List<Defense>(); //populate this list each time you create a building.
	public GameObject spawnerLayer;
	Sprite Sprite;

	void Start(){
		spawnerLayer = new GameObject("Spawns");		
	}
}
