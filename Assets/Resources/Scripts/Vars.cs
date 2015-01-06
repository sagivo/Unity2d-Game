using UnityEngine;
using System.Collections;

public class Vars {
	public struct PrefabPaths{
		[System.NonSerialized]
		public static string cell = "Prefabs/Cell";
		[System.NonSerialized]
		public static string uiHealthbar = "Prefabs/UI/HealthBar";
	}

	public struct Balance{
		public struct Player{
			public struct AutoCanon{
				[System.NonSerialized]
				public static int[] buildCostPerLevel = new int[]{5,10,20,40,80};
				[System.NonSerialized]
				public static int[] refundPerLevel = new int[]{5,10,20,40,80};
				[System.NonSerialized]
				public static float[] shootSpeedPerLevel = new float[]{1, .7f, .3f, .2f};
				[System.NonSerialized]
				public static float[] upgradeTimePerLevel = new float[]{2,5,10,20};
			}

			public struct MineralMiner{
				[System.NonSerialized]
				public static int[] buildCostPerLevel = new int[]{10,20,40,80};
				[System.NonSerialized]
				public static int[] refundPerLevel = new int[]{5,10,20,40,80};
				[System.NonSerialized]
				public static float[] upgradeTimePerLevel = new float[]{2,5,10,20};
				[System.NonSerialized]
				public static float[] timeToMineralPerLevel = new float[]{2, 1, .7f, .5f};
			}
			public struct Cell{
				[System.NonSerialized]
				public static int[] expendCostPerLevel = new int[]{25,30,50,1};
			}
		}
	}
}