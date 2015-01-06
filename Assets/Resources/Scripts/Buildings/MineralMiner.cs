using UnityEngine;
using System.Collections;

public class MineralMiner : Building {
	public float[] timeToMineralPerLevel = Vars.Balance.Player.MineralMiner.timeToMineralPerLevel;

	protected new void Awake(){
		base.Awake();
		buildCostPerLevel = Vars.Balance.Player.MineralMiner.buildCostPerLevel;
		buildTimePerLevel = Vars.Balance.Player.MineralMiner.upgradeTimePerLevel;
		refundPerLevel = Vars.Balance.Player.MineralMiner.refundPerLevel;
	}


	protected new void Start () {
		base.Start();
		Game.mineralMiners.Add (this);
		InvokeRepeating("getMinerals",0,timeToMineralPerLevel[level]);
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
