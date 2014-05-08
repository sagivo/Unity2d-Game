using UnityEngine;
using System.Collections;

public class Canon : Liveable {

	void Start(){
		Game.Canons.Add(this);
	}
	
	void OnDestroy(){
		Game.Canons.Remove(this);
	}

}
