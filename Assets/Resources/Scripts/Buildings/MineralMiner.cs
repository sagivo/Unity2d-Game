using UnityEngine;
using System.Collections;

public class MineralMiner : Liveable {
	public float timeToMineral = 5;

	protected new void Start () {
		base.Start();
		Game.mineralMiners.Add (this);
		InvokeRepeating("getMinerals",0,timeToMineral);
	}
	
	protected new void Update () {
		base.Update();
	}

	void getMinerals(){
		Game.minerals++;
	}
}
