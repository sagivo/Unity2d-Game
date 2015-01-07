using UnityEngine;
using System.Collections;
using Newtonsoft.Json;

public class Config {
	public static Config.RootObject _obj;
	public static Config.RootObject GetConfig(){
		if (_obj == null){
			string t = (Resources.Load("Config/settings") as TextAsset).text;
			_obj = JsonConvert.DeserializeObject<Config.RootObject>(t);
		} return _obj;
	}

	public class PrefabPaths
	{
		public string cell { get; set; }
		public string uiHealthbar { get; set; }
	}
	
	public class PlayerCanon
	{
		public int[] healthPerLevel { get; set; }
	}
	
	public class AutoCanon
	{
		public int[] buildCostPerLevel { get; set; }
		public int[] refundPerLevel { get; set; }
		public float[] shootSpeedPerLevel { get; set; }
		public float[] upgradeTimePerLevel { get; set; }
		public int[] healthPerLevel { get; set; }
	}
	
	public class MineralMiner
	{
		public int[] buildCostPerLevel { get; set; }
		public int[] refundPerLevel { get; set; }
		public float[] upgradeTimePerLevel { get; set; }
		public float[] timeToMineralPerLevel { get; set; }
		public int[] healthPerLevel { get; set; }
	}
	
	public class Cell
	{
		public int[] expendCostPerLevel { get; set; }
		public int[] upgradeTimePerLevel { get; set; }
	}
	
	public class Building
	{
	}
	
	public class Player
	{
		public PlayerCanon PlayerCanon { get; set; }
		public AutoCanon AutoCanon { get; set; }
		public MineralMiner MineralMiner { get; set; }
		public Cell Cell { get; set; }
		public Building Building { get; set; }
	}
	
	public class CanonDestroyer
	{
		public int[] healthPerLevel { get; set; }
		public float[] shootRangePerLevel { get; set; }
		public float[] shootSpeedPerLevel { get; set; }
	}
	
	public class Kamikazi
	{
		public int[] healthPerLevel { get; set; }
	}
	
	public class Enemy
	{
		public CanonDestroyer canonDestroyer { get; set; }
		public Kamikazi kamikazi { get; set; }
	}
	
	public class Balance
	{
		public Player Player { get; set; }
		public Enemy Enemy { get; set; }
	}
	
	public class RootObject
	{
		public PrefabPaths PrefabPaths { get; set; }
		public Balance Balance { get; set; }
	}
}
