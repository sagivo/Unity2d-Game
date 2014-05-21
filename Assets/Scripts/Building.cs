using UnityEngine;
using System.Collections;

public class Building : Liveable {

	protected new void Start(){
		base.Start();
		Game.Buildings.Add(this);
	}
	
	void OnDestroy(){
		Game.Buildings.Remove(this);
	}

}
