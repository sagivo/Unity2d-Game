using UnityEngine;
using System.Collections;

public class MineralMiner : Building {
	public float[] timeToMineralPerLevel;


	protected new void Awake(){
		base.Awake();
	}


	protected new void Start () {
		base.Start();

		buildCostPerLevel = Vars.Balance.Player.MineralMiner.buildCostPerLevel;
		healthPerLevel = Vars.Balance.Player.MineralMiner.healthPerLevel;
		buildTimePerLevel = Vars.Balance.Player.MineralMiner.upgradeTimePerLevel;
		refundPerLevel = Vars.Balance.Player.MineralMiner.refundPerLevel;
		timeToMineralPerLevel = Vars.Balance.Player.MineralMiner.timeToMineralPerLevel;

		Game.mineralMiners.Add (this);
		build();

		OnStatusChange += (s) => {
			if (status == StatusType.Live){
				CancelInvoke("getMinerals");
				InvokeRepeating("getMinerals",0,timeToMineralPerLevel[level]);
			}
		};
	}

	protected new void Update () {
		base.Update();
	}

	void getMinerals(){
		Game.minerals++;
	}
}
