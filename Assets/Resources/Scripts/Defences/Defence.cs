using UnityEngine;
using System.Collections;

public class Defence : Liveable {

	protected  new void Start () {
		base.Start();

		Game.Defences.Add (this);
		OnDie += () => { Game.Defences.Remove (this); };
	}

}
