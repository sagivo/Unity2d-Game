using UnityEngine;
using System.Collections;

public class Defence : Liveable {

	protected  new void Start () {
		base.Start();

		Game.defences.Add (this);
		OnDie += () => { Game.defences.Remove (this); };
	} 

}
