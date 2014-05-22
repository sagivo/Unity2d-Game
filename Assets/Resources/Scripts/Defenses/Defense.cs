using UnityEngine;
using System.Collections;

public class Defense : Liveable {

	protected  new void Start () {
		base.Start();
		Game.Defenses.Add (this);

		OnDie += () => { Game.Defenses.Remove (this); };
	}

}
