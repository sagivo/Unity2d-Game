using UnityEngine;
using System.Collections;
using System;

using System.IO;
using System.Xml.Serialization;

public class Vars2 {

	public PrefabPaths prefabPaths = new PrefabPaths();
	public class PrefabPaths{
		public string cell = "Prefabs/Cell";
		public string uiHealthbar = "Prefabs/UI/HealthBar";
	}

	public Balance balance = new Balance();
	public class Balance{
		public Player player = new Player();
		public class Player{
			public PlayerCanon playerCanon = new PlayerCanon();
			public class PlayerCanon{
				public int[] healthPerLevel = new int[]{200,500,10000,20000};
			}
			public AutoCanon autoCanon = new AutoCanon();
			public class AutoCanon{
				public int[] buildCostPerLevel = new int[]{5,10,20,40,80}; //public int [] xxx {get{return  
				public int[] refundPerLevel = new int[]{5,10,20,40,80};
				public float[] shootSpeedPerLevel = new float[]{1, .7f, .3f, .2f};
				public float[] upgradeTimePerLevel = new float[]{2,5,10,20};
				public int[] healthPerLevel = new int[]{12,15,110,120};
			}

			public MineralMiner mineralMiner = new MineralMiner();
			public class MineralMiner{
				public int[] buildCostPerLevel = new int[]{10,20,40,80};
				public int[] refundPerLevel = new int[]{5,10,20,40,80};
				public float[] upgradeTimePerLevel = new float[]{2,5,10,20};
				public float[] timeToMineralPerLevel = new float[]{2, 1, .7f, .5f};
				public int[] healthPerLevel = new int[]{2,5,10,20};
			}
			public Cell cell = new Cell();
			public class Cell{
				public int[] expendCostPerLevel = new int[]{25,30,50,1};
				public float[] upgradeTimePerLevel = new float[]{2,5,10,20};
			}

			public Building building = new Building();
			public class Building{
			}
		}

		public Enemy enemy = new Enemy();
		public class Enemy{
			public CanonDestroyer canonDestroyer = new CanonDestroyer();
			public class CanonDestroyer{
				public int[] healthPerLevel = new int[]{5000,800, 1200};
				public float[] shootRangePerLevel = new float[]{10,30};
				public float[] shootSpeedPerLevel = new float[]{112, 1.7f, 1.5f, 1f};
				
			}
			public Kamikazi kamikazi = new Kamikazi();
			public class Kamikazi{
				public int[] healthPerLevel = new int[]{40,80,100};
			}
		}
	}




	
}