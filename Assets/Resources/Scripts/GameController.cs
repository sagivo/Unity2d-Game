using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameController : Singleton<GameController> {
	public int level;
	public Player player;
	public int score;
	public List<Resource> resources = new List<Resource>();
	public List<Cell> cells = new List<Cell>();
	public List<Enemy> enemies = new List<Enemy>(); //populate this list each time you create an enemy.
	public List<Building> buildings = new List<Building>(); //populate this list each time you create a building.
	public List<Canon> canons = new List<Canon>(); //populate this list each time you create a building.
	public List<MineralMiner> mineralMiners = new List<MineralMiner>(); //populate this list each time you create a building.
	public List<Defence> defences = new List<Defence>(); //populate this list each time you create a building.
	public GameObject spawnerLayer;
	public CellMenue menue;
	Sprite Sprite;
	//resources
	public int minerals = 50;
	public int diamonds = 3;

	new void Awake(){
		base.Awake();
		menue = GameObject.Find("CellMenue").GetComponent<CellMenue>();
	}

	new void Start(){
		base.Start();
		spawnerLayer = new GameObject("Spawns");	
		Game.cells[0].expend();
	}
}
