using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameController : Singleton<GameController> {
	public int level;
	public GameObject player;
	public int score;
	public List<Resource> resources = new List<Resource>();
	public List<Cell> cells = new List<Cell>();
	public List<Enemy> enemies = new List<Enemy>(); //populate this list each time you create an enemy.
	public List<Building> buildings = new List<Building>(); //populate this list each time you create a building.
	public List<AutoCanon> autoCanons = new List<AutoCanon>(); //populate this list each time you create a building.
	public List<MineralMiner> mineralMiners = new List<MineralMiner>(); //populate this list each time you create a building.
	public GameObject spawnerLayer;
	public CellMenue menue;
	public System.Action OnMineralChange;
	Sprite Sprite;
	//resources
	int _minerals;
	public int minerals {
		get{return _minerals;}
		set
		{_minerals = value;if (OnMineralChange!=null) OnMineralChange();}
	}
	public int diamonds = 3;

	new void Awake(){
		base.Awake();
		if (GameObject.Find("CellMenue")) menue = GameObject.Find("CellMenue").GetComponent<CellMenue>();
	}

	new void Start(){
		base.Start();
		spawnerLayer = new GameObject("Spawns");	
	}

	public bool canBuild(Building o){
		return (Game.minerals >= o.mineralCostBuild || Game.diamonds >= o.diamondCost);
	}

	public bool canUpgrade(Building o){
		return (Game.minerals >= o.mineralCostUpgrade[level]);
	}

	public void setTimeScale(float scale){
		Time.timeScale = scale;
	}
}
