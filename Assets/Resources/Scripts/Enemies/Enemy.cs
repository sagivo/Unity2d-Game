using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Enemy : Liveable 
{
	protected GameObject target;
	[Header("Enemy")]
	public int costKill = 1;
	protected int[] killBonusMinerals = new int[]{1,5,10};
	protected int[] damagePerLevel;

	protected new void Start(){
		base.Start();
		Game.enemies.Add(this);

		OnDie += () => {
			Game.minerals += killBonusMinerals[level];
		};
	}

	void OnDestroy()
	{
		Game.enemies.Remove(this);
	}

}
