using UnityEngine;
using System.Collections;

public class Temp : BaseObj {

	// Use this for initialization
	new void Awake () {
		base.Awake();
		Vars = Config.GetConfig();

	}
	
	new void Start () {
		base.Start();
		Application.LoadLevel("lvl1");
	}
}
