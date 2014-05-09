using UnityEngine;
using System.Collections;

public class Building : Liveable {

	void Start(){
		Game.Buildings.Add(this);
	}
	
	void OnDestroy(){
		Game.Buildings.Remove(this);
	}

}
