using UnityEngine;
using System.Collections;

public class MineralMiner : Building {
	public float[] timeToMineralPerLevel;

	protected new void Awake(){
		base.Awake();
	}

	protected new void Start () {
		base.Start();

		Game.mineralMiners.Add (this);
		build();

		OnStatusChange += (s) => {
			if (status == StatusType.Live){
				CancelInvoke("getMinerals");
				InvokeRepeating("getMinerals",0,timeToMineralPerLevel[level]);
			}
		};

		OnDie += () => {
			Game.mineralMiners.Remove(this);
		};
	}

	protected new void Update () {
		base.Update();
	}

	void getMinerals(){
		Game.minerals++;
	}
}
