using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

using System.IO;
using System.Xml.Serialization;

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
	[System.NonSerialized]
	int _minerals = 10;

	public int minerals {
		get{return _minerals;}
		set{_minerals = value; if (OnMineralChange!=null) OnMineralChange();}
	}
	public int diamonds = 3;

	new void Awake(){
		base.Awake();
		if (GameObject.Find("CellMenue")) menue = GameObject.Find("CellMenue").GetComponent<CellMenue>();
		spawnerLayer = new GameObject("Spawns");
		spawnerLayer.transform.parent = transform;


		var serializer = new XmlSerializer(typeof(Vars2));
		var stream = new FileStream("Assets/Resources/Config/xml.xml", FileMode.Create);
		serializer.Serialize(stream, new Vars2());
		stream.Close();

	}

	new void Start(){
		base.Start();
	}

	
	new void Update(){
		base.Update();
	}

	public bool canUpgrade(Building o){
		return (Game.minerals >= o.buildCostPerLevel[level]);
	}

	public void setTimeScale(float scale){
		Time.timeScale = scale;
	}


}
