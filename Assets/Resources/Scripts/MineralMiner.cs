using UnityEngine;
using System.Collections;

public class MineralMiner : Liveable {

	protected new void Start () {
		base.Start();
		Game.mineralMiners.Add (this);			
	}
	
	protected new void Update () {
		base.Update();
	}
}
