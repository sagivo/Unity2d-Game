using UnityEngine;
using System.Collections;

public class Vars {
	public struct PrefabPaths{
		public static string cell = "Prefabs/Cell";
		public static string uiHealthbar = "Prefabs/UI/HealthBar";
	}

	public struct Balance{
		public struct Player{
			public struct AutoCanon{
				public static int[] mineralCostUpgrade = new int[]{5,10,20,40,80};
				public static float[] shootSpeedPerLevel = new float[]{1, .7f, .3f, .2f};
				public static float[] upgradeTimePerLevel = new float[]{2,5,10,20};
			}

			public struct MineralMiner{
				public static int[] mineralCostUpgrade = new int[]{5,10,20,40,80};
				public static float[] upgradeTimePerLevel = new float[]{2,5,10,20};
				public static float[] timeToMineralPerLevel = new float[]{2, 1, .7f, .5f};
			}
		}
	}
}
