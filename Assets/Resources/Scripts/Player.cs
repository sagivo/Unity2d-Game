using UnityEngine;
using System.Collections;

public class Player : BaseObj {

	// Use this for initialization
	new void Start () {
		base.Start();
		Game.player = gameObject;
	}
	
}
