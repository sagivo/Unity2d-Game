using UnityEngine;
using System.Collections;

public class Building : Liveable {

	protected new void Start(){
		base.Start();
		Game.buildings.Add(this);
	}
	
	void OnDestroy(){
		Game.buildings.Remove(this);
	}

}
