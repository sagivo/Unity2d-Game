using UnityEngine;
using System.Collections;

public class Kamikazi : Enemy {

	void Start () {
		findTarget();
	}

	void Update () {
	
	}

	void findTarget() {
		Target = Helpers.CloestToObject(Game.Canons.ToArray(), gameObject);
		if (Target == null) Invoke("findTarget", 2);		
	}
}
